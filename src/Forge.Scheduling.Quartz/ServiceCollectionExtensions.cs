using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Quartz;

namespace Forge.Scheduling.Quartz;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddQuartzJobs(
        this IServiceCollection services,
        Action<OptionsBuilder<QuartzOptions>> jobsRegistration,
        Action<IServiceCollectionQuartzConfigurator>? configure = null)
    {
        services.AddQuartz(opt =>
        {
            opt.UseMicrosoftDependencyInjectionJobFactory();
            opt.UseSimpleTypeLoader();
            opt.UseInMemoryStore();
            opt.UseDefaultThreadPool();

            configure?.Invoke(opt);
        });

        services.AddQuartzHostedService(opt => opt.WaitForJobsToComplete = true);

        var optionsBuilder = services.AddOptions<QuartzOptions>();
        jobsRegistration.Invoke(optionsBuilder);
        return services;
    }
}
