using RabbitMQ.Client;

namespace Forge.MessageBroker.RabbitMQ.Connections
{
    internal sealed class SubscribeConnection : ISubscribeConnection
    {
        public IConnection Connection { get; }

        public SubscribeConnection(IConnection connection)
        {
            Connection = connection;
        }
    }
}
