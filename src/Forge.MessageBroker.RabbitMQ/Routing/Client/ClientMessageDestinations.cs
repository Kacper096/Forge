namespace Forge.MessageBroker.RabbitMQ.Routing.Client
{
    internal sealed class ClientMessageDestinations : IClientMessageDestinations
    {
        private readonly IDictionary<Type, IMessageDestination> _destinations = new Dictionary<Type, IMessageDestination>();

        public void Add(Type messageType, IMessageDestination destination)
        {
            if (_destinations.Any(dest => dest.GetType() == messageType))
            {
                _destinations.Remove(messageType);
                _destinations.Add(messageType, destination);
                return;
            }

            _destinations.Add(messageType, destination);
        }

        public void Add<TMessage>(IMessageDestination destination)
            where TMessage : IMessage
        {
            var type = typeof(TMessage);
            if (_destinations.Any(dest => dest.GetType() == type))
            {
                _destinations.Remove(type);
                _destinations.Add(type, destination);
                return;
            }

            _destinations.Add(type, destination);
        }

        public IMessageDestination this[Type messageType]
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

        public IList<IMessageDestination> GetAll()
        {
            return _destinations.Select(x => x.Value).ToList();
        }
    }
}
