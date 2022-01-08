namespace Forge.MessageBroker.RabbitMQ.Exchange
{
    public interface IExchangeOptionsInitializer
    {
        string Name { get; set; }
        string Type { get; set; }
    }
}