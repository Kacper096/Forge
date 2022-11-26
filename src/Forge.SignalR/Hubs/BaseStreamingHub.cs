using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Forge.SignalR.Hubs;

public abstract class BaseStreamingHub : BaseHub
{
    protected BaseStreamingHub(
        IOptionsMonitor<StreamingOptions> streamingOptions,
        ILogger<BaseStreamingHub> logger)
        : base(logger)
    {
        Options = streamingOptions.CurrentValue;
    }

    protected StreamingOptions Options { get; }

    protected async IAsyncEnumerable<TResponse> Stream<TResponse>(
        Func<TResponse> getFunc,
        [EnumeratorCancellation] CancellationToken cancellationToken)
        where TResponse : class, new()
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            yield return TryGet(getFunc);
            await Task.Delay(Options.Interval, cancellationToken);
        }
    }

    private TResponse TryGet<TResponse>(Func<TResponse> getFunc)
        where TResponse : class, new()
    {
        try
        {
            return getFunc();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Streaming error during getting data.");
            return new TResponse();
        }
    }
}
