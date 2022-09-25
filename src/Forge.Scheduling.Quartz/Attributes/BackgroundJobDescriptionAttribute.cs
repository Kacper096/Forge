namespace Forge.Scheduling.Quartz.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class BackgroundJobDescriptionAttribute : Attribute
{
    public BackgroundJobDescriptionAttribute(
        string name,
        string? group = null,
        string? description = null)
    {
        Name = name;
        Group = group;
        Description = description;
    }

    public string Name { get; }
    public string? Group { get; set; }
    public string? Description { get; set; }
}
