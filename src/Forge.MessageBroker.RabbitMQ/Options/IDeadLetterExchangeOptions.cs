namespace Forge.MessageBroker.RabbitMQ.Options
{
    public interface IDeadLetterExchangeOptions
    {
        bool Enabled { get; set; }
        string PrefixName { get; set; }
    }
}
