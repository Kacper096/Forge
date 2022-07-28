
namespace Forge.MessageBroker.RabbitMQ.Routing.Client
{
    internal interface IClientMessageDestinations
    {
        IMessageDestination this[Type messageType] { get; set; }

        void Add(Type messageType, IMessageDestination destinations);
        void Add<TMessage>(IMessageDestination destinations) where TMessage : IMessage;
        IMessageDestination? Get(Type messageType);
        IMessageDestination? Get<TMessage>() where TMessage : IMessage;
        IList<IMessageDestination> GetAll();
    }
}