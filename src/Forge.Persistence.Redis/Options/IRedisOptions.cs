namespace Forge.Persistence.Redis.Options
{
    public interface IRedisOptions
    {
        string? ConnectionString { get; set; }
    }
}