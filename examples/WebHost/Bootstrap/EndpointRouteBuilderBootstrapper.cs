using Forge.Application.Commands;
using Forge.Application.Data.Measurements;
using Forge.Application.Queries;

namespace Forge.WebHost.Bootstrap;

public static class EndpointRouteBuilderBootstrapper
{
    public static void Bootstrap(IEndpointRouteBuilder endpoints)
    {
        endpoints.Post<AddTemperatureMeasurementCommand>("/temperature")
                 .Post<AddTemperatureToCacheCommand>("/temperature-cache");

        endpoints.Get<GetTemperatureFromCacheQuery, Temperature>("/temperature-cache");
    }
}
