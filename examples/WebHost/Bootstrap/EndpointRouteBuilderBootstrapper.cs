using Forge.Application.Commands;

namespace Forge.WebHost.Bootstrap;

public static class EndpointRouteBuilderBootstrapper
{
    public static void Bootstrap(IEndpointRouteBuilder endpoints)
    {
        endpoints.Post<AddTemperatureMeasurementCommand>("/temperature");
    }
}
