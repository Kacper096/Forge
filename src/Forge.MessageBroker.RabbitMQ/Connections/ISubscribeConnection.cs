using RabbitMQ.Client;

namespace Forge.MessageBroker.RabbitMQ.Connections
{
    internal interface ISubscribeConnection
    {
        IConnection Connection { get; }
    }
}
