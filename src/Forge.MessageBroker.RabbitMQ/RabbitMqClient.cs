using Forge.MessageBroker.RabbitMQ.Connections;
using Forge.MessageBroker.RabbitMQ.Routing.Client;
using Forge.MessageBroker.RabbitMQ.Serializers;

namespace Forge.MessageBroker.RabbitMQ
{
    internal sealed class RabbitMqClient : IRabbitMqClient
    {
        private readonly IRabbitMessageSerializer _serializer;
        private readonly IClientMessageDestinations _messageDestinations;
        private readonly IPublishConnection _publishConnection;

        public RabbitMqClient(IRabbitMessageSerializer serializer,
                              IClientMessageDestinations messageDestinations,
                              IPublishConnection publishConnection)
        {
            _serializer = serializer;
            _messageDestinations = messageDestinations;
            _publishConnection = publishConnection;
        }

        public void Send(IMessage message)
        {
            var body =  _serializer.Serialize(message);

            using var channel = _publishConnection.Connection.CreateModel();
            var messageDestination = _messageDestinations.Get(message.GetType());
            channel.BasicPublish(messageDestination.Exchange, messageDestination.RoutingKey, false, null, body.ToArray());
        }
    }
}
