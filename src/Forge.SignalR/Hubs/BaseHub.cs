using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Forge.SignalR.Hubs;

public abstract class BaseHub : Hub
{
    protected BaseHub(ILogger<BaseHub> logger)
    {
        Logger = logger;
        Info = new HubInfo(GetType());
    }

    protected ILogger<BaseHub> Logger { get; }
    protected HubInfo Info { get; }

    public override Task OnConnectedAsync()
    {
        Logger.LogInformation("Hub - Name: '{0}' Group: '{1}' Description: '{2}' established connection: {3}",
            Info.DisplayName,
            Info.Group,
            Info.Description,
            Context.ConnectionId);
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception exception)
    {
        Logger.LogInformation("Hub - Name: '{0}' Group: '{1}' Description: '{2}' disconnected: {3}",
            Info.DisplayName,
            Info.Group,
            Info.Description,
            Context.ConnectionId);
        return base.OnDisconnectedAsync(exception);
    }
}
