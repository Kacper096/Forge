namespace Forge.MessageBroker.RabbitMQ
{
    internal static class RabbitHandlerUtil
    {
        public static Action<IMessage> BuildHandleMethod(object? handler)
        {
            var handleMethod = handler?.GetType()?.GetMethod(nameof(IHandle<IMessage>.Handle));
            return (message) => handleMethod?.Invoke(handler, new object[] { message });
        }
    }
}
