using Microsoft.AspNetCore.Builder;

namespace Forge.SignalR;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseSignalR(this IApplicationBuilder app)
    {
        app.UseEndpoints(endpoints => endpoints.MapHubs());

        return app;
    }
}
