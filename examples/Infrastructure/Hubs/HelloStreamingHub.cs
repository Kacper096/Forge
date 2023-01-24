using Forge.SignalR;
using Forge.SignalR.Attributes;
using Forge.SignalR.Hubs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Forge.Infrastructure.Hubs;

[Hub(name: nameof(HelloStreamingHub), displayName: "Hello Streaming Hub", group: "Hello Group", description: "This hub sends 'Hello World' in streaming mode.")]
public class HelloStreamingHub : BaseStreamingHub
{
    public HelloStreamingHub(
        IOptionsMonitor<StreamingOptions> streamingOptions,
        ILogger<HelloStreamingHub> logger)
        : base(streamingOptions, logger)
    {
    }

    public IAsyncEnumerable<HelloStreamingResponse> Hello([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        return Stream(GetResponse, cancellationToken);

        static HelloStreamingResponse GetResponse() =>
            new(){ Name = "Hello World" };
    }
}

public class HelloStreamingResponse
{
    public string Name { get; set; }
}
