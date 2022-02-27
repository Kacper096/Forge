using Forge.MessageBroker.RabbitMQ.Options;

namespace Forge.MessageBroker.RabbitMQ.Exchange
{
    internal static class ExchangeOptionsInitializerFactory
    {
        internal static IExchangeOptionsInitializer CreateInstance(IExchangeOptionsInitializer existingInstance)
        {
            existingInstance ??= new ExchangeOptionsInitializer();
            existingInstance.DeadLetterExchangeOptions = new DeadLetterExchangeOptions();
            return existingInstance;
        }
    }
}
