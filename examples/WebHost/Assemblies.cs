using Forge.Application.Queries;
using System.Reflection;

namespace Forge.WebHost;

public static class Assemblies
{
    public static Assembly Application => typeof(GetTemperatureFromCacheQuery).Assembly;
}
