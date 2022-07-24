using System.ComponentModel.DataAnnotations.Schema;

namespace HBDStack.EfCore.Abstractions.Attributes;

/// <summary>
/// This attribute will load the enums into a table in database.
/// </summary>
[AttributeUsage(AttributeTargets.Enum)]
public sealed class StaticDataAttribute : TableAttribute
{
    public StaticDataAttribute(string name) : base(name)
    {
    }
}