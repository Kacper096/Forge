using Forge.Scheduling.Quartz;
using Forge.Scheduling.Quartz.Attributes;
using Microsoft.Extensions.Logging;
using Quartz;
using System.Threading.Tasks;

namespace Forge.Infrastructure.Jobs;

[DisallowConcurrentExecution]
[BackgroundJobDescription("HelloJob", "LoggingGroup", "Logging hello word.")]
public sealed class HelloJob : IBackgroundJob
{
    private readonly ILogger<HelloJob> _logger;

    public HelloJob(ILogger<HelloJob> logger)
    {
        _logger = logger;
    }

    public Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Hello from HelloJob");
        return Task.CompletedTask;
    }
}
