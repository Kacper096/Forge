using Forge.MessageBroker.RabbitMQ;

namespace Forge.Application.Messages;

public class RabbitTestMessage : IMessage
{
    public string Name { get; set; } = string.Empty;
}
