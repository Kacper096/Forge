using Forge.Infrastructure.Jobs;
using Forge.Scheduling.Quartz;

namespace Forge.WebHost.Bootstrap;

public static class BackgroundJobBootstrapper
{
    public static void Bootstrap(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<HelloJobOptions>(configuration.GetSection(HelloJobOptions.DefaultSectionName));
        services.AddQuartzJobs(reg => reg.RegisterCronJob<HelloJob, HelloJobOptions>());
    }
}
