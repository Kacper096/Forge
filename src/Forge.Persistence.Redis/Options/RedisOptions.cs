namespace Forge.Persistence.Redis.Options
{
    public class RedisOptions : IRedisOptions
    {
        public const string SectionName = "redis";

        public string? ConnectionString { get; set; }
    }
}
