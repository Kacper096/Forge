using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Forge;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddForge(
        this IServiceCollection services,
        IConfiguration configuration, 
        string appName = ApplicationOptions.DefaultSectionName)
    {
        services.Configure<ApplicationOptions>(configuration.GetSection(appName));
        return services;
    }
}
