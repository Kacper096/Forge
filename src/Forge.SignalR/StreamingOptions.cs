using System;

namespace Forge.SignalR;

public class StreamingOptions
{
    internal const string SectionName = "streaming-hubs";

    public TimeSpan Interval { get; set; } = new TimeSpan(0, 0, 1);
}
