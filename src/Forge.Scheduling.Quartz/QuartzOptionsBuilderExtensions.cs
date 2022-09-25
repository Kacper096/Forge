using Forge.Scheduling.Quartz.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Quartz;

namespace Forge.Scheduling.Quartz;

public static class QuartzOptionsBuilderExtensions
{
    public static OptionsBuilder<QuartzOptions> RegisterCronJob<TJob, TOptions>(
        this OptionsBuilder<QuartzOptions> builder)
        where TJob : class, IBackgroundJob
        where TOptions : class, ICronOptions, new()
    {
        builder.Services.AddScoped<TJob>();
        builder.Configure<IOptions<TOptions>>(
            (quartz, opt) => quartz.RegisterJob<TJob>(opt.Value.Cron));

        return builder;
    }

    public static OptionsBuilder<QuartzOptions> RegisterIntervalJob<TJob, TOptions>(
        this OptionsBuilder<QuartzOptions> builder)
        where TJob : class, IBackgroundJob
        where TOptions : class, IIntervalOptions, new()
    {
        builder.Services.AddScoped<TJob>();
        builder.Configure<IOptions<TOptions>>(
            (quartz, opt) => quartz.RegisterJob<TJob>(opt.Value.Interval));

        return builder;
    }
}
