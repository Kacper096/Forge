using Forge.MessageBroker.RabbitMQ.Options;

namespace Forge.MessageBroker.RabbitMQ.Exchange
{
    public sealed record class ExchangeOptionsInitializer : IExchangeOptionsInitializer
    {
        private const string DefaultType = "topic";
        public string Type { get; set; } = DefaultType;
        public string Name { get; set; }
        public IDeadLetterExchangeOptions DeadLetterExchangeOptions { get; set; }
    }
}
