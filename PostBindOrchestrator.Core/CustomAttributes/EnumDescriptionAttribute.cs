namespace PostBindOrchestrator.Core;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
public sealed class EnumDescriptionAttribute : Attribute
{
    public string Description { get; }

    public EnumDescriptionAttribute(string description)
    {
        Description = description;
    }
}
