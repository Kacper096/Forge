using Forge.Application.Queries;
using Forge.ContextIntegration;
using System.Reflection;

namespace Forge.WebHost;

public static class Assemblies
{
    public static Assembly Application => typeof(GetTemperatureFromCacheQuery).Assembly;
    public static Assembly ContextIntegration => typeof(ContextIntegrationMarker).Assembly;
}
