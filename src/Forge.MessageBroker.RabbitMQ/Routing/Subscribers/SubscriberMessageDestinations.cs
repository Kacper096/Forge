namespace Forge.MessageBroker.RabbitMQ.Routing.Subscribers
{
    internal sealed class SubscriberMessageDestinations : ISubscriberMessageDestinations
    {
        private readonly IDictionary<Type, IMessageDestination> _destinations = new Dictionary<Type, IMessageDestination>();

        public void Add(Type messageType, IMessageDestination destination)
        {
            _destinations.Add(messageType, destination);
        }
        public void Add<TMessage>(IMessageDestination destination)
            where TMessage : IMessage
        {
            _destinations.Add(typeof(TMessage), destination);
        }

        public IMessageDestination? this[Type messageType]
        {
            get => _destinations[messageType];
            set => _destinations[messageType] = value;
        }

        public IMessageDestination? Get<TMessage>()
            where TMessage : IMessage
        {
            return _destinations.TryGetValue(typeof(TMessage), out var destination) ? destination : null;
        }

        public IMessageDestination? Get(Type messageType)
        {
            return _destinations.TryGetValue(messageType, out var destination) ? destination : null;
        }

        public IEnumerable<IMessageDestination> GetAll()
        {
            return _destinations.Values;
        }
        public IDictionary<Type, IMessageDestination> GetAllWithTypes()
        {
            return _destinations;
        }

        public void Remove<TMessage>()
            where TMessage : IMessage
        {
            var messageType = typeof(TMessage);
            if (!_destinations.Any(d => d.Key == messageType))
            {
                return;
            }
            _destinations.Remove(messageType);
        }
    }
}
