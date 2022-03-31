using StackExchange.Redis;

namespace Forge.Persistence.Redis.Connections
{
    public interface IConnection
    {
        IConnectionMultiplexer Connection { get; }
        string Name { get; }
    }
}