using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Forge.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.SignalR;

namespace Forge.SignalR;

internal static class EndpointRouteBuilderExtensions
{
    internal static IEndpointRouteBuilder MapHubs(this IEndpointRouteBuilder endpointRouteBuilder,
        params Assembly[] assemblies)
    {
        var hubInfos = assemblies.SelectMany(a => a.GetTypes())
            .Where(t => t.Inherits<Hub>())
            .Select(hubType => new HubInfo(hubType))
            .ToArray();

        ValidateHubs(hubInfos);

        foreach (var hubInfo in hubInfos)
        {
            endpointRouteBuilder.MapHub(hubInfo);
        }

        return endpointRouteBuilder;
    }

    private static HubEndpointConventionBuilder MapHub(this IEndpointRouteBuilder endpointRouteBuilder, HubInfo hubInfo)
    {
        var hubExtensionType = typeof(HubEndpointRouteBuilderExtensions);
        var mapHubMethod = hubExtensionType.GetMethod(nameof(HubEndpointRouteBuilderExtensions.MapHub),
            BindingFlags.Public | BindingFlags.Static, new[] { typeof(IEndpointRouteBuilder), typeof(string) });
        var genericMapHub = mapHubMethod!.MakeGenericMethod(hubInfo.HubType)!;

        var hubBuilder = (genericMapHub.Invoke(null, new object[] { endpointRouteBuilder, hubInfo.EndpointName }) as HubEndpointConventionBuilder)!;
        hubBuilder.WithGroupName(hubInfo.Group)
                  .WithDisplayName(hubInfo.DisplayName);

        return hubBuilder;
    }

    private static void ValidateHubs(IReadOnlyCollection<HubInfo> hubInfos)
    {
        var uniqueNameExceptions = hubInfos.GroupBy(h => h.Name)
            .Where(g => g.Count() > 0)
            .Select(g => g.First().Name)
            .Join(hubInfos,
                  duplicatedName => duplicatedName,
                  hubInfo => hubInfo.Name,
                  (duplicatedName, hubInfo) => hubInfo)
            .Select(info => new Exception($"Hub type: {info.HubType.FullName} with name: {info.Name} must have unique name."))
            .ToArray();
        if (uniqueNameExceptions.Any())
        {
            throw new AggregateException("SignalR cannot map hubs.", uniqueNameExceptions);
        }
    }
}
