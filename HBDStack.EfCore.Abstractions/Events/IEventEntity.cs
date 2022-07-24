namespace HBDStack.EfCore.Abstractions.Events;

/// <summary>
///     The Domain Entity Interface.
///     The Domain Entity will support to raise domain events.
/// </summary>
public interface IEventEntity
{
    /// <summary>
    ///     Add event to queue.
    /// </summary>
    /// <param name="event"></param>
    void AddEvent(IEventItem @event);

    /// <summary>
    /// Add event type to the queue and this will be map from entity when pushing the events.
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    void AddEvent<TEvent>() where TEvent : IEventItem;

    /// <summary>
    ///  Get and Clear all event in queue.
    /// </summary>
    /// <returns></returns>
    (IEventItem[]? events, Type[]? eventTypes) GetEventsAndClear();
}