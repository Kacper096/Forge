using Forge.MessageBroker.RabbitMQ.Options;
using RabbitMQ.Client;

namespace Forge.MessageBroker.RabbitMQ
{
    internal sealed class RabbitMqConnectionFactory : IRabbitMqConnectionFactory
    {
        private readonly IRabbitMQOptions _options;

        public RabbitMqConnectionFactory(IRabbitMQOptions options)
        {
            _options = options;
        }

        public IConnection Generate(string suffixConnectionName)
        {
            var factory = new ConnectionFactory()
            {
                HostName = _options.HostName,
                Port = _options.Port,
                UserName = _options.UserName,
                Password = _options.Password,
                VirtualHost = _options.VirtualHost,
                DispatchConsumersAsync = true,
            };

            return factory.CreateConnection(hostnames: new List<string>()
            {
                _options.HostName,
            }, clientProvidedName: $"{_options.ConnectionName}_{suffixConnectionName}");
        }
    }
}
