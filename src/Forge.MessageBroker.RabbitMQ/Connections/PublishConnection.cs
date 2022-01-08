using RabbitMQ.Client;

namespace Forge.MessageBroker.RabbitMQ.Connections
{
    internal sealed class PublishConnection : IPublishConnection
    {
        public IConnection Connection { get; }

        public PublishConnection(IConnection connection)
        {
            Connection = connection;
        }
    }
}
