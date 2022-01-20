using Forge.Exceptions;
using Forge.MessageBroker.RabbitMQ.Connections;
using Forge.MessageBroker.RabbitMQ.Exchange;
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

        private readonly IServiceProvider _serviceProvider;
        private readonly IConnection _subscribeConnection;
        private readonly IRabbitMessageSerializer _serializer;
        private readonly ISubscriberMessageDestinations _messageDestinations;
        private readonly ILogger<RabbitMqSubscribersService> _logger;
        private readonly IExchangeInitializer _exchangeInitializer;

        public RabbitMqSubscribersService(IServiceProvider serviceProvider,
                                          ISubscribeConnection subscribeConnection,
                                          IRabbitMessageSerializer serializer,
                                          ISubscriberMessageDestinations messageDestinations,
                                          ILogger<RabbitMqSubscribersService> logger,
                                          IExchangeInitializer exchangeInitializer)
        {
            _serviceProvider = serviceProvider;
            _subscribeConnection = subscribeConnection.Connection;
            _serializer = serializer;
            _messageDestinations = messageDestinations;
            _logger = logger;
            _exchangeInitializer = exchangeInitializer;
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
            channel.QueueDeclare(messageDestination.Queue, autoDelete: false, exclusive: false);
            channel.QueueBind(messageDestination.Queue, messageDestination.Exchange, messageDestination.RoutingKey);

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

        private async Task TryHandleAsync(IModel channel, Action<IMessage> handle, IMessage message,
            BasicDeliverEventArgs args)
        {
            var currentRetry = 0;
            var retry = Policy.Handle<Exception>().WaitAndRetryAsync(Retries, _ => TimeSpan.FromSeconds(RetryInterval));

            await retry.ExecuteAsync(async () =>
            {
                try
                {
                    handle?.Invoke(message);
                    channel.BasicAck(args.DeliveryTag, false);
                    await Task.Yield();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, message: ex.Message);
                    currentRetry++;

                    if (ex.IsDomainOrAppException())
                    {
                        //todo: add mechanism to return app or domain exception
                        channel.BasicAck(args.DeliveryTag, false);
                        await Task.Yield();
                        return;
                    }

                    channel.BasicNack(args.DeliveryTag, false, false);
                    await Task.Yield();
                }
            });
        }
    }
}
