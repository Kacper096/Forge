using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Forge.SignalR;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddStreamingSignalR(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<StreamingOptions>(configuration.GetSection(StreamingOptions.SectionName));

        return services;
    }

    public static IApplicationBuilder UseSignalR(this IApplicationBuilder app)
    {
        app.UseEndpoints(endpoints => endpoints.MapHubs());

        return app;
    }
}
