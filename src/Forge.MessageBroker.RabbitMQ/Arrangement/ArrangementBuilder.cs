using Forge.MessageBroker.RabbitMQ.Exchange;

namespace Forge.MessageBroker.RabbitMQ.Arrangement
{
    internal sealed class ArrangementBuilder : IArrangementBuilder
    {
        private readonly IExchangeOptionsInitializer _options;

        private const string _queueTemplate = "[assembly]/[exchange].[name]";

        public ArrangementBuilder(IExchangeOptionsInitializer options)
        {
            _options = options;
        }

        public string GetRoutingKey(Type type, string routingKeyOverwrite)
        {
            if (string.IsNullOrWhiteSpace(routingKeyOverwrite))
            {
                return type.Name;
            }
            return routingKeyOverwrite;
        }

        public string GetExchange(Type type, string exchangeOverwrite)
        {
            if (string.IsNullOrWhiteSpace(exchangeOverwrite))
            {
                return _options.Name ?? type.Assembly.GetName().Name;
            }
            return exchangeOverwrite;
        }

        public string GetQueue(Type type, string exchangeOverwrite, string queueOverWrite, bool completlyOverwriteQueue = false)
        {
            var isQueueEmpty = string.IsNullOrWhiteSpace(queueOverWrite);
            if (completlyOverwriteQueue && !isQueueEmpty)
            {
                return queueOverWrite;
            }

            var assembly = type.Assembly.GetName().Name;
            var exchange = GetExchange(type, exchangeOverwrite);
            var name = isQueueEmpty ? type.Name : queueOverWrite;

            return _queueTemplate.Replace("[assembly]", assembly)
                .Replace("[exchange]", exchange)
                .Replace("[name]", name);
        }

    }
}
