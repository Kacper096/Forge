using Forge.SignalR;
using Forge.SignalR.Attributes;
using Forge.SignalR.Hubs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Forge.Infrastructure.Hubs;

[Hub(nameof(HelloStreamingHub))]
public class HelloStreamingHub : BaseStreamingHub
{
    public HelloStreamingHub(
        IOptionsMonitor<StreamingOptions> streamingOptions,
        ILogger<BaseStreamingHub> logger)
        : base(streamingOptions, logger)
    {
    }

    public IAsyncEnumerable<HelloStreamingResponse> Hello([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        return Stream(GetResponse, cancellationToken);

        HelloStreamingResponse GetResponse() =>
            new HelloStreamingResponse { Name = "Hello World" };
    }
}

public class HelloStreamingResponse
{
    public string Name { get; set; }
}
