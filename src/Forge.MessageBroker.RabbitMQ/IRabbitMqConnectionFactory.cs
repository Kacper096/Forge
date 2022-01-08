using RabbitMQ.Client;

namespace Forge.MessageBroker.RabbitMQ
{
    internal interface IRabbitMqConnectionFactory
    {
        IConnection Generate(string suffixConnectionName);
    }
}