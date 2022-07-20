using Forge.Application.Messages;
using Forge.ContextIntegration.Shared.Handlers;
using Microsoft.Extensions.Logging;

namespace Forge.ContextIntegration.Handlers;

internal class RabbitTestMessageHandler : BaseExternalMessageHandler<RabbitTestMessage>
{
    public RabbitTestMessageHandler(ILogger<BaseExternalMessageHandler<RabbitTestMessage>> logger) 
        : base(logger)
    {
    }

    protected override void OnHandle(RabbitTestMessage message)
    {
        Logger.LogInformation("Handled message name: {0}", message.Name);
    }
}
