using Forge.Exceptions;
using Forge.MessageBroker.RabbitMQ;
using Microsoft.Extensions.Logging;

namespace Forge.ContextIntegration.Shared.Handlers;

internal abstract class BaseExternalMessageHandler<TMessage> : IHandle<TMessage>
    where TMessage : IMessage
{
    protected ILogger<BaseExternalMessageHandler<TMessage>> Logger { get; }

    public BaseExternalMessageHandler(ILogger<BaseExternalMessageHandler<TMessage>> logger)
    {
        Logger = logger;
    }

    public void Handle(TMessage message)
        => TryDo(() => OnHandle(message));

    protected abstract void OnHandle(TMessage message);

    private void TryDo(Action handleMessage)
    {
        try
        {
            handleMessage.Invoke();
        }
        catch (DomainException domainException)
        {
            Logger.LogError(domainException, "Occured domain exception during handling external message.");
        }
        catch(AppException appException)
        {
            Logger.LogError(appException, "Occured app exception during handling external message.");
        }
    }
}
