using Forge.MessageBroker.RabbitMQ.Arrangement;
using Forge.MessageBroker.RabbitMQ.Routing.Client;
using Forge.MessageBroker.RabbitMQ.Routing.Subscribers;

namespace Forge.MessageBroker.RabbitMQ.Routing
{
    internal sealed class MessageDestinationProvider : IConsumerMessageDestinationProvider, IPublisherMessageDestinationProvider, IDisposable
    {
        private readonly IArrangementBuilder _arrangementBuilder;

        private readonly ICollection<IMessageDestinationDefinedInConfiguration> _subscriberConfigurationMessages = new HashSet<IMessageDestinationDefinedInConfiguration>();
        private readonly ICollection<IMessageDestinationDefinedInConfiguration> _clientConfigurationMessages = new HashSet<IMessageDestinationDefinedInConfiguration>();

        public MessageDestinationProvider(IArrangementBuilder arrangementBuilder)
        {
            _arrangementBuilder = arrangementBuilder;
        }

        IPublisherMessageDestinationProvider IPublisherMessageDestinationProvider.Add<TMessage>(string exchange = null, string routingKey = null)
        {
            var type = typeof(TMessage);
            _clientConfigurationMessages.Add(new MessageDestinationDefinedInConfiguration(type, routingKey, exchange, null));
            return this;
        }

        public IConsumerMessageDestinationProvider Add<TMessage>(string routingKey = null, string exchange = null, string queue = null)
            where TMessage : IMessage
        {
            var type = typeof(TMessage);

            _subscriberConfigurationMessages.Add(new MessageDestinationDefinedInConfiguration(type, routingKey, exchange, queue));
            return this;
        }

        internal void ApplyDestinations(ISubscriberMessageDestinations subscriberDestinations, IClientMessageDestinations clientMessageDestinations)
        {
            AddMessageDestinations(subscriberDestinations.Add, _subscriberConfigurationMessages);
            AddMessageDestinations(clientMessageDestinations.Add, _clientConfigurationMessages);
        }

        private void AddMessageDestinations(Action<Type, MessageDestination> addAction,IEnumerable<IMessageDestinationDefinedInConfiguration> configurations)
        {
            foreach(var configurationMessage in configurations)
            {
                var routingKey = _arrangementBuilder.GetRoutingKey(configurationMessage.MessageType, configurationMessage.RoutingKey);
                var exchange = _arrangementBuilder.GetExchange(configurationMessage.MessageType, configurationMessage.Exchange);
                var queue = _arrangementBuilder.GetQueue(configurationMessage.MessageType, configurationMessage.Exchange, configurationMessage.Queue);

                addAction.Invoke(configurationMessage.MessageType, new MessageDestination(routingKey, exchange, queue));
            }
        }

        public void Dispose()
        {
            _subscriberConfigurationMessages.Clear();
            _clientConfigurationMessages.Clear();
            GC.SuppressFinalize(this);
        }
    }
}
