
namespace Forge.MessageBroker.RabbitMQ.Routing.Subscribers
{
    internal interface ISubscriberMessageDestinations
    {
        IMessageDestination? this[Type messageType] { get; set; }

        void Add(Type messageType, IMessageDestination destination);
        void Add<TMesasge>(IMessageDestination destination) where TMesasge : IMessage;
        IMessageDestination? Get(Type messageType);
        IMessageDestination? Get<TMessage>() where TMessage : IMessage;
        IEnumerable<IMessageDestination> GetAll();
        IDictionary<Type, IMessageDestination> GetAllWithTypes();
        void Remove<TMessage>() where TMessage : IMessage;
    }
}