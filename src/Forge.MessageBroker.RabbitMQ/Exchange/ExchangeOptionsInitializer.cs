using Forge.MessageBroker.RabbitMQ.Options;
using System.Reflection;

namespace Forge.MessageBroker.RabbitMQ.Exchange
{
    public sealed record class ExchangeOptionsInitializer : IExchangeOptionsInitializer
    {
        private const string DefaultType = "topic";
        public string Type { get; set; } = DefaultType;
        public string Name { get; set; }
        public IDeadLetterExchangeOptions DeadLetterExchangeOptions { get; set; }

        internal ExchangeOptionsInitializer(string? appName)
        {
            Name = string.IsNullOrWhiteSpace(appName) ? Assembly.GetExecutingAssembly().GetName().Name! : appName;
            DeadLetterExchangeOptions = new DeadLetterExchangeOptions();
        }
    }
}
