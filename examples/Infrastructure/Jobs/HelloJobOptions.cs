using Forge.Scheduling.Quartz.Options;

namespace Forge.Infrastructure.Jobs;

public class HelloJobOptions : ICronOptions
{
    public const string DefaultSectionName = "hello-job";
    public string Cron { get; set; }
}
