using Forge.MessageBroker.RabbitMQ.Options;

namespace Forge.MessageBroker.RabbitMQ.Exchange
{
    public interface IExchangeOptionsInitializer
    {
        string Name { get; set; }
        string Type { get; set; }
        IDeadLetterExchangeOptions DeadLetterExchangeOptions { get; set; }
    }
}