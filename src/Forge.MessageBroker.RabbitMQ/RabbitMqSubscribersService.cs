using Forge.Exceptions;
using Forge.MessageBroker.RabbitMQ.Connections;
using Forge.MessageBroker.RabbitMQ.Exchange;
using Forge.MessageBroker.RabbitMQ.Options;
using Forge.MessageBroker.RabbitMQ.Routing;
using Forge.MessageBroker.RabbitMQ.Routing.Subscribers;
using Forge.MessageBroker.RabbitMQ.Serializers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Reflection;

namespace Forge.MessageBroker.RabbitMQ
{
    internal sealed class RabbitMqSubscribersService : BackgroundService
    {
        private const int Retries = 3;
        private const double RetryInterval = 10;
        private const string DeadLetterExchange = "x-dead-letter-exchange";
        private const string DeadLetterRouting = "x-dead-letter-routing-key";

        private readonly IServiceProvider _serviceProvider;
        private readonly IConnection _subscribeConnection;
        private readonly IRabbitMessageSerializer _serializer;
        private readonly ISubscriberMessageDestinations _messageDestinations;
        private readonly ILogger<RabbitMqSubscribersService> _logger;
        private readonly IExchangeInitializer _exchangeInitializer;
        private readonly IDeadLetterExchangeOptions _deadLetterOptions;

        public RabbitMqSubscribersService(IServiceProvider serviceProvider,
                                          ISubscribeConnection subscribeConnection,
                                          IRabbitMessageSerializer serializer,
                                          ISubscriberMessageDestinations messageDestinations,
                                          ILogger<RabbitMqSubscribersService> logger,
                                          IExchangeInitializer exchangeInitializer,
                                          IDeadLetterExchangeOptions deadLetterOptions)
        {
            _serviceProvider = serviceProvider;
            _subscribeConnection = subscribeConnection.Connection;
            _serializer = serializer;
            _messageDestinations = messageDestinations;
            _logger = logger;
            _exchangeInitializer = exchangeInitializer;
            _deadLetterOptions = deadLetterOptions;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _exchangeInitializer.Initialize();
            return Task.Run(() =>
            {
                foreach ((Type type, IMessageDestination messageDestination) in _messageDestinations.GetAllWithTypes())
                {
                    ListenForMessage(type, messageDestination);
                }
            }, stoppingToken);
        }

        private void ListenForMessage(Type messageType, IMessageDestination messageDestination)
        {
            var channel = _subscribeConnection.CreateModel();
            var queueArgs = GetQueueArguments(messageDestination);
            channel.QueueDeclare(messageDestination.Queue, autoDelete: false, exclusive: false, arguments: queueArgs);
            channel.QueueBind(messageDestination.Queue, messageDestination.Exchange, messageDestination.RoutingKey);
            _logger.LogInformation($"Queue [{messageDestination.Queue}] has been declared for an exchange [{messageDestination.Exchange}]");
            DeclareDeadLetterQueue(channel, messageDestination, queueArgs);

            var genericHandlerType = typeof(IHandle<>);
            var handlerType = genericHandlerType.MakeGenericType(messageType);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += async (_, args) =>
            {
                try
                {
                    var handler = _serviceProvider.GetService(handlerType);
                    var handleMethod = RabbitHandlerUtil.BuildHandleMethod(handler);
                    var message = _serializer.Deserialize(args.Body.Span, messageType) as IMessage;
                    
                    await TryHandleAsync(channel, handleMethod, message, args);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    channel.BasicNack(args.DeliveryTag, false, false);
                    await Task.Yield();
                }
            };

            channel.BasicConsume(messageDestination.Queue, false, consumer);
        }

        private async Task TryHandleAsync(IModel channel, Action<IMessage, Action<Exception>> handle, IMessage message,
            BasicDeliverEventArgs args)
        {
            var currentRetry = 0;
            var retry = Policy.Handle<Exception>().WaitAndRetryAsync(Retries, _ => TimeSpan.FromSeconds(RetryInterval));

            await retry.ExecuteAsync(async () =>
            {
                handle?.Invoke(message, async (ex) =>
                {
                    _logger.LogError(ex, message: ex.Message);
                    currentRetry++;

                    var messageType = message.GetType().Name;
                    var messageCont = message.ToString();
                    var errorMessage = _deadLetterOptions.Enabled ? $"Message [{messageType}]: {messageCont} will be moved to dead letter" : $"Unhandled message [{messageType}]: {messageCont}";

                    _logger.LogError(errorMessage);
                    channel.BasicNack(args.DeliveryTag, false, false);
                    await Task.Yield();
                });
                channel.BasicAck(args.DeliveryTag, false);
                await Task.Yield();
            });
        }

        private IDictionary<string, object>? GetQueueArguments(IMessageDestination messageDestination)
        {
            return !_deadLetterOptions.Enabled
                ? new Dictionary<string, object>()
                : new Dictionary<string, object>()
                    {
                        { DeadLetterExchange, $"{_deadLetterOptions.PrefixName}{messageDestination.Exchange}" },
                        { DeadLetterRouting, $"{_deadLetterOptions.PrefixName}{messageDestination.Queue}" }
                    };
        }

        private void DeclareDeadLetterQueue(IModel channel, IMessageDestination messageDestination, IDictionary<string, object> queueArguments)
        {
            if (!_deadLetterOptions.Enabled)
            {
                return;
            }

            var queueName = queueArguments[DeadLetterRouting] as string;
            var exchangeName = queueArguments[DeadLetterExchange] as string;
            var deadLetterArgs = new Dictionary<string, object>()
            {
                { DeadLetterExchange, messageDestination.Exchange },
                { DeadLetterRouting, messageDestination.Queue }
            };

            channel.QueueDeclare(queueName, autoDelete: false, exclusive: false, arguments: deadLetterArgs);
            channel.QueueBind(queueName, exchangeName, queueName);
            _logger.LogInformation($"Deadletter queue [{queueName}] has been declared for an exchange [{exchangeName}]");
        }
    }
}
