using System.Reflection;

namespace Forge.MessageBroker.RabbitMQ
{
    internal static class RabbitHandlerUtil
    {
        public static Action<IMessage, Action<Exception>> BuildHandleMethod(object? handler)
        {
            var handleMethod = handler?.GetType()?.GetMethod(nameof(IHandle<IMessage>.Handle));
            return (message, handleError) =>
            {
                try
                {
                    handleMethod?.Invoke(handler, new object[] { message });
                }
                catch (TargetInvocationException ex)
                {
                    handleError?.Invoke(obj: ex.InnerException);
                }
            }; 
        }
    }
}
