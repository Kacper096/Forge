namespace Forge.MessageBroker.RabbitMQ.Routing;

public interface IPublisherMessageDestinationProvider
{
    IPublisherMessageDestinationProvider Add<TMessage>(string? routingKey = null, string? exchange = null) where TMessage : IMessage;
}
