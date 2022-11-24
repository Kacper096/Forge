using System;

namespace Forge.SignalR.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class HubAttribute : Attribute
{
    public HubAttribute(
        string name,
        string? displayName,
        string? group,
        string? description)
    {
        Name = name;
        DisplayName = displayName;
        Group = group;
        Description = description;
    }

    public string Name { get; }
    public string? DisplayName { get; }
    public string? Group { get; }
    public string? Description { get; }
}
