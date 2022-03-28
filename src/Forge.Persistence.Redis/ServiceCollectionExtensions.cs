using Forge.Persistence.Redis.Connections;
using Forge.Persistence.Redis.Options;
using Forge.Persistence.Redis.Serializers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Forge.Persistence.Redis
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
        {
            var redisOptions = configuration.GetOptions<RedisOptions>(RedisOptions.SectionName);
            services.AddSingleton<IRedisOptions>(redisOptions)
                    .AddSingleton<IRModelSerializer, RModelSerializer>();

            var connectionFactory = new RConnectionFactory(redisOptions);
            var connection = connectionFactory.Generate();

            services.AddSingleton<IConnection>(connection)
                    .AddTransient<IRService, RService>();
            return services;
        }
    }
}
