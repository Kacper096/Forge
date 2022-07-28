namespace Forge.MessageBroker.RabbitMQ.Routing;

public interface IConsumerMessageDestinationProvider
{
    IConsumerMessageDestinationProvider Add<TMessage>(string? routingKey = null, string? exchange = null, string? queue = null) where TMessage : IMessage;
}
