using RabbitMQ.Client;

namespace Forge.MessageBroker.RabbitMQ.Connections
{
    internal interface IPublishConnection
    {
        IConnection Connection { get; }
    }
}
