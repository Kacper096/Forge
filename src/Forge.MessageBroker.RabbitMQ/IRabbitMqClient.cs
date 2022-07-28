namespace Forge.MessageBroker.RabbitMQ;

public interface IRabbitMqClient
{
    void Send(IMessage message);
}
