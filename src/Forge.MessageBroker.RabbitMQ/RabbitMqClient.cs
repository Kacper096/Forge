using Forge.MessageBroker.RabbitMQ.Connections;
using Forge.MessageBroker.RabbitMQ.Routing.Client;
using Forge.MessageBroker.RabbitMQ.Serializers;
using Microsoft.Extensions.Logging;

namespace Forge.MessageBroker.RabbitMQ;

internal sealed class RabbitMqClient : IRabbitMqClient
{
    private readonly IRabbitMessageSerializer _serializer;
    private readonly IClientMessageDestinations _messageDestinations;
    private readonly IPublishConnection _publishConnection;
    private readonly ILogger<RabbitMqClient> _logger;

    public RabbitMqClient(IRabbitMessageSerializer serializer,
                          IClientMessageDestinations messageDestinations,
                          IPublishConnection publishConnection,
                          ILogger<RabbitMqClient> logger)
    {
        _serializer = serializer;
        _messageDestinations = messageDestinations;
        _publishConnection = publishConnection;
        _logger = logger;
    }

    public void Send(IMessage message)
    {
        var body =  _serializer.Serialize(message);

        using var channel = _publishConnection.Connection.CreateModel();
        var messageDestination = _messageDestinations.Get(message.GetType());
        if (messageDestination is null)
        {
            _logger.LogWarning("Sending message: {FullName} will be skipped, because not specified destination.", message.GetType().FullName);
            return;
        }
        channel.BasicPublish(messageDestination.Exchange, messageDestination.RoutingKey, false, null, body.ToArray());
    }
}
