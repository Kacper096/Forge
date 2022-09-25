using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Threading.Tasks;

namespace Forge.Scheduling.Quartz.Jobs;

internal sealed class LoggingJob<TJob> : IBackgroundJob
    where TJob : IBackgroundJob
{
    private readonly IJob _job;
    private readonly ILogger<LoggingJob<TJob>> _logger;

    public LoggingJob(TJob job, ILogger<LoggingJob<TJob>> logger)
    {
        _job = job;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
        => await RunJob(context);

    private async Task RunJob(IJobExecutionContext context)
    {
        var jobName = context.JobDetail.Key.Name;
        try
        {
            _logger.LogDebug("Starting background job: {jobName}",  jobName);
            await _job.Execute(context);
            _logger.LogDebug("Ending the execution background job: {jobName}",  jobName);
        }
        catch (AggregateException aggregateEx)
        {
            var innerEx = aggregateEx.InnerException ?? new Exception($"Occured {nameof(AggregateException)} but inner exception is null.");
            LogDefaultErrorMessage(innerEx, jobName);
        }
        catch (TaskCanceledException taskCancelEx)
        {
            LogDefaultCancelledMessage(taskCancelEx, jobName);
        }
        catch (OperationCanceledException canceledEx)
        {
            LogDefaultCancelledMessage(canceledEx, jobName);
        }
        catch (Exception ex)
        {
            LogDefaultErrorMessage(ex, jobName);
        }
    }

    private void LogDefaultErrorMessage(Exception exception, string jobName)
    {
        _logger.LogError(
            exception: exception,
            message: "Exception occured when executing background job of name {jobName}.",
            args: jobName);
    }

    private void LogDefaultCancelledMessage(Exception exception, string jobName)
    {
        _logger.LogWarning(
            exception: exception,
            message: "Background job: {jobName} cancelled.",
            args: jobName);
    }
}
