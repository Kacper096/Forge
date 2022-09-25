using Forge.Scheduling.Quartz.Attributes;
using Quartz;
using System;
using System.Reflection;

namespace Forge.Scheduling.Quartz;

public record JobInfo
{
    public JobInfo(Type jobType)
    {
        Type = jobType;
        var backgroundJobDescription = jobType.GetCustomAttribute<BackgroundJobDescriptionAttribute>(inherit: false);
        JobKey = GetJobKey(backgroundJobDescription);
        Description = CreateJobDescription(backgroundJobDescription);
        TriggerKey = CreateTriggerKey(backgroundJobDescription);
        Group = CreateGroupName(backgroundJobDescription);
    }

    public Type Type { get; }
    public JobKey JobKey { get; }
    public TriggerKey TriggerKey { get; }
    public string? Description { get; }
    public string Group { get; }

    private JobKey GetJobKey(BackgroundJobDescriptionAttribute? attribute)
        => attribute is null ?
            new JobKey(Type.FullName!) :
            new JobKey(attribute.Name);

    private TriggerKey CreateTriggerKey(BackgroundJobDescriptionAttribute? attribute)
        => attribute is null ?
            new TriggerKey(CreateTriggerName(Type.FullName!), CreateGroupName(attribute)) :
            new TriggerKey(CreateTriggerName(attribute.Name), CreateGroupName(attribute));

    private static string? CreateJobDescription(BackgroundJobDescriptionAttribute? attribute)
        => attribute?.Description;

    private static string CreateGroupName(BackgroundJobDescriptionAttribute? attribute)
        => attribute?.Description ?? "GlobalGroup";

    private static string CreateTriggerName(string jobName)
        => $"{jobName}Trigger";
}
