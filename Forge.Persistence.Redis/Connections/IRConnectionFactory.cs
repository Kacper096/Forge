namespace Forge.Persistence.Redis.Connections
{
    internal interface IRConnectionFactory
    {
        RConnection Generate(string name = "redis-connection");
    }
}