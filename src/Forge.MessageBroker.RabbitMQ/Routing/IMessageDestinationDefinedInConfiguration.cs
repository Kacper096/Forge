using Forge.MessageBroker.RabbitMQ.Routing.Client;
using Forge.MessageBroker.RabbitMQ.Routing.Subscribers;

namespace Forge.MessageBroker.RabbitMQ.Routing
{
    internal interface IMessageDestinationDefinedInConfiguration
    {
        Type MessageType { get; }
        string Exchange { get; }
        string Queue { get; }
        string RoutingKey { get; }
    }
}