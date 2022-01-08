namespace Forge.MessageBroker.RabbitMQ.Routing
{
    internal interface IMessageDestination
    {
        string Exchange { get; }
        string Queue { get; }
        string RoutingKey { get; }

        bool Equals(MessageDestination? other);
        bool Equals(object? obj);
        int GetHashCode();
        string ToString();
    }
}