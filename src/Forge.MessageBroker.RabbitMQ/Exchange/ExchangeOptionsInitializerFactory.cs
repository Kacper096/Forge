using Forge.MessageBroker.RabbitMQ.Options;

namespace Forge.MessageBroker.RabbitMQ.Exchange
{
    internal static class ExchangeOptionsInitializerFactory
    {
        internal static IExchangeOptionsInitializer CreateInstance(IExchangeOptionsInitializer existingInstance, string? appName)
        {
            existingInstance ??= new ExchangeOptionsInitializer(appName);
            return existingInstance;
        }
    }
}
