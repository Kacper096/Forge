using Forge.Persistence.InfluxDb.Connections;
using Forge.Persistence.InfluxDb.Models;
using Forge.Persistence.InfluxDb.Options;
using Forge.Persistence.InfluxDb.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Forge.Persistence.InfluxDb
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfluxDb(this IServiceCollection services, IConfiguration configuration)
        {
            var influxOptions = configuration.GetOptions<InfluxOptions>(InfluxOptions.SectionName);
            var healthCheckState = new HealthCheckState();
            services.AddSingleton<IInfluxOptions>(influxOptions)
                    .AddSingleton<IHealthCheckState>(healthCheckState)
                    .AddSingleton<ISetHealthCheckState>(healthCheckState);

            var connectionFactory = new InfluxConnectionFactory(influxOptions);
            var connection = connectionFactory.Create();

            services.AddSingleton<IInfluxConnection>(connection);
            services.AddTransient<IInfluxService, InfluxService>();
            services.AddHostedService<HealthCheckBackgroundService>();
            return services;
        }
    }
}
