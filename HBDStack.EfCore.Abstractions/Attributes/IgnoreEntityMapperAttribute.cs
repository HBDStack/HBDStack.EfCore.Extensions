namespace HBDStack.EfCore.Abstractions.Attributes;

/// <summary>
///     Allow to ignore the Entity class from auto entity mapper
///     This should be use for delivered types.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class IgnoreEntityMapperAttribute : Attribute
{
}