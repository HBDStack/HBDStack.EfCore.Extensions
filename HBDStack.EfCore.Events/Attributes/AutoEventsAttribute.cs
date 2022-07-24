using HBDStack.EfCore.Abstractions.Events;

namespace HBDStack.EfCore.Events.Attributes;

/// <summary>
/// Auto add event to entity based event types provided. All event type MUST be a <see cref="IEventItem"/>
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class AutoEventsAttribute : Attribute
{
    public Type? CreatedEventType { get; set; }
    public Type? UpdatedEventType { get; set; }
    public Type? DeletedEventType { get; set; }
}