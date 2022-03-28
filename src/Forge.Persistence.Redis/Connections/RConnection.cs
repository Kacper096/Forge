using StackExchange.Redis;

namespace Forge.Persistence.Redis.Connections
{
    public sealed class RConnection : IConnection
    {
        public string Name { get; }
        public IConnectionMultiplexer Connection { get; }

        public RConnection(string name, IConnectionMultiplexer connection)
        {
            Name = name;
            Connection = connection;
        }
    }
}
