namespace Forge.MessageBroker.RabbitMQ
{
    public interface IHandle<TMessage> where TMessage : IMessage
    {
        void Handle(TMessage message);
    }
}
