
namespace Forge.MessageBroker.RabbitMQ.Arrangement
{
    internal interface IArrangementBuilder
    {
        string GetExchange(Type type, string exchangeOverwrite);
        string GetQueue(Type type, string exchangeOverwrite, string queueOverWrite, bool completlyOverwriteQueue = false);
        string GetRoutingKey(Type type, string routingKeyOverwrite);
    }
}