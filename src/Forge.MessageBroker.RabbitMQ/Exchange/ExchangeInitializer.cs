using Forge.MessageBroker.RabbitMQ.Connections;
using Forge.MessageBroker.RabbitMQ.Routing.Subscribers;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Forge.MessageBroker.RabbitMQ.Exchange
{
    internal sealed class ExchangeInitializer : IExchangeInitializer
    {
        private readonly IExchangeOptionsInitializer _options;
        private readonly IPublishConnection _publishConnection;
        private readonly ISubscriberMessageDestinations _subscribeDestinations;
        private readonly ILogger<ExchangeInitializer> _logger;

        public ExchangeInitializer(IExchangeOptionsInitializer options,
                                   IPublishConnection publishConnection,
                                   ISubscriberMessageDestinations subscribeDestinations,
                                   ILogger<ExchangeInitializer> logger)
        {
            _options = options;
            _publishConnection = publishConnection;
            _subscribeDestinations = subscribeDestinations;
            _logger = logger;
        }

        public void Initialize()
        {
            var type = _options.Type;
            var exchanges = _subscribeDestinations.GetAll()
                .Where(m => !string.Equals(m.Exchange, _options.Name, StringComparison.InvariantCultureIgnoreCase))
                .GroupBy(m => m.Exchange)
                .Select(g => g.First().Exchange)
                .ToList();

            using var channel = _publishConnection.Connection.CreateModel();
            channel.ExchangeDeclare(_options.Name, type, durable: true, autoDelete: false, arguments: null);
            LogExchange(_options.Name, type);
            DeclareDeadLetterExchange(channel);

            exchanges.ForEach(name =>
            {
                channel.ExchangeDeclare(name, type, durable: true, autoDelete: false, arguments: null);
                LogExchange(name, type);
            });
        }

        private void DeclareDeadLetterExchange(IModel channel)
        {
            if (!_options.DeadLetterExchangeOptions.Enabled)
            {
                return;
            }

            var exchangeName = $"{_options.DeadLetterExchangeOptions.PrefixName}{_options.Name}";
            var exchangeType = ExchangeType.Direct;
            channel.ExchangeDeclare(exchangeName, exchangeType, durable: true, autoDelete: false, arguments: null);
            LogExchange(exchangeName, exchangeType);
        }

        private void LogExchange(string name, string type) => _logger.LogInformation($"Exchange: {name} with type: {type} has been declared.");
    }
}
