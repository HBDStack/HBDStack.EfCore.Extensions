using HBDStack.EfCore.Abstractions.Events;

namespace HBDStack.EfCore.Events;

/// <summary>
///     The Entity and Events information
/// </summary>
public sealed class EntityEventItem
{
    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="entity">The entity instance</param>
    /// <param name="events">The events of entity</param>
    public EntityEventItem(IEventEntity entity, params IEventItem[] events)
    {
        Entity = entity ?? throw new ArgumentNullException(nameof(entity));
        Events = new List<IEventItem>(events);
    }

    public IEventEntity Entity { get; }
    public ICollection<IEventItem> Events { get; }
}