namespace Forge.MessageBroker.RabbitMQ.Routing;

internal interface IMessageDestinationDefinedInConfiguration
{
    Type MessageType { get; }
    string? Exchange { get; }
    string? Queue { get; }
    string? RoutingKey { get; }
}
