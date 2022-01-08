using Forge.MessageBroker.RabbitMQ.Routing.Client;
using Forge.MessageBroker.RabbitMQ.Routing.Subscribers;

namespace Forge.MessageBroker.RabbitMQ.Routing
{
    internal sealed record class MessageDestinationDefinedInConfiguration : IMessageDestinationDefinedInConfiguration
    {
        public Type MessageType { get; }
        public string RoutingKey { get; }
        public string Exchange { get; }
        public string Queue { get; }

        public MessageDestinationDefinedInConfiguration(Type messageType, string routingKey, string exchange, string queue)
        {
            MessageType = messageType;
            RoutingKey = routingKey;
            Exchange = exchange;
            Queue = queue;
        }
    }
}
