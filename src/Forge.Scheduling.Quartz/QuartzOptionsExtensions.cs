using Forge.Scheduling.Quartz.Jobs;
using Quartz;
using System;

namespace Forge.Scheduling.Quartz;

public static class QuartzOptionsExtensions
{
    public static QuartzOptions RegisterJob<TJob>(
        this QuartzOptions options,
        string cron)
        where TJob : class, IBackgroundJob
    {
        if (string.IsNullOrWhiteSpace(cron))
            throw new ArgumentException($"Quartz .NET cannot schedule job {typeof(TJob).FullName} because cron is empty.");

        return options.RegisterJob<TJob>(cron, interval: TimeSpan.Zero);
    }

    public static QuartzOptions RegisterJob<TJob>(
        this QuartzOptions options,
        TimeSpan interval)
        where TJob : class, IBackgroundJob
        => options.RegisterJob<TJob>(interval);

    private static QuartzOptions RegisterJob<TJob>(
        this QuartzOptions options,
        string? cron,
        TimeSpan interval)
        where TJob : class, IBackgroundJob
    {
        var isCronEmpty = string.IsNullOrWhiteSpace(cron);
        var jobInfo = new JobInfo(typeof(TJob));

        options.AddJob<LoggingJob<TJob>>(opt => opt
            .WithIdentity(jobInfo.JobKey)
            .WithDescription(jobInfo.Description));

        options.AddTrigger(opt =>
        {
            opt.ForJob(jobInfo.JobKey)
               .WithIdentity(jobInfo.TriggerKey)
               .WithDescription(jobInfo.Description);

            if (!isCronEmpty)
            {
                opt.WithCronSchedule(cron!);
                return;
            }

            opt.WithSimpleSchedule(sb => sb.WithInterval(interval).RepeatForever());
        });

        return options;
    }
}
