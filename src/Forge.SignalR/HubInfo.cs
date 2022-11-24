using Forge.SignalR.Attributes;
using System;
using System.Reflection;

namespace Forge.SignalR;

public record HubInfo
{
    private const string HubPathPrefix = "hubs";
    private const string DefaultGroupName = "GlobalGroup";
    private const string DefaultDescription = "Empty";

    public HubInfo(Type hubType)
    {
        HubType = hubType;
        var hubAttribute = hubType.GetCustomAttribute<HubAttribute>(inherit: false);
        Name = CreateName(hubAttribute);
        DisplayName = CreateDisplayName(hubAttribute);
        Group = CreateGroup(hubAttribute);
        Description = CreateDescription(hubAttribute);
        EndpointName = CreateEndpointName(hubAttribute);
    }

    public Type HubType { get; }
    public string Name { get; }
    public string DisplayName { get; }
    public string Group { get; }
    public string Description { get; }
    public string EndpointName { get; }

    private string CreateName(HubAttribute? attribute)
        => attribute?.Name ?? HubType.Name;

    private string CreateDisplayName(HubAttribute? attribute)
        => attribute?.DisplayName ?? CreateName(attribute);

    private string CreateEndpointName(HubAttribute? attribute)
        => $"/{HubPathPrefix}/{CreateName(attribute)}";

    private static string CreateGroup(HubAttribute? attribute)
        => attribute?.Group ?? DefaultGroupName;

    private static string CreateDescription(HubAttribute? attribute)
        => attribute?.Description ?? DefaultDescription;
}
