namespace Forge.MessageBroker.RabbitMQ.Routing
{
    public interface IPublisherMessageDestinationProvider
    {
        IPublisherMessageDestinationProvider Add<TMessage>(string exchange = null, string routingKey = null) where TMessage : IMessage;
    }
}
