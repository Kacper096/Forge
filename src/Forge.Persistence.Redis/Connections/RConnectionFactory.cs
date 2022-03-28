using Forge.Persistence.Redis.Options;
using StackExchange.Redis;

namespace Forge.Persistence.Redis.Connections
{
    internal sealed class RConnectionFactory : IRConnectionFactory
    {
        private readonly IRedisOptions _options;

        public RConnectionFactory(IRedisOptions options)
        {
            _options = options;
        }

        public RConnection Generate(string name = "redis-connection")
        {
            var multiplexerConnection = ConnectionMultiplexer.Connect(_options.ConnectionString, (opt) =>
            {
                opt.ClientName = name;
            });
            return new RConnection(name, multiplexerConnection);
        }
    }
}
