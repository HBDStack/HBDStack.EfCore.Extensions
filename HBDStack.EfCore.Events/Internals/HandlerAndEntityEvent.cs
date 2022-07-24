using HBDStack.EfCore.Abstractions.Events;

namespace HBDStack.EfCore.Events.Internals;

internal class HandlerAndEntityEvent
{
    public HandlerAndEntityEvent(HandlerTypes type, dynamic handler, IEventEntity entity, IEventItem @event)
    {
        Type = type;
        Handler = handler;
        Entity = entity;
        Event = @event;
    }

    public HandlerTypes Type { get; }
    public dynamic Handler { get; }
    public IEventEntity Entity { get; }
    public IEventItem Event { get; }

    /// <summary>
    ///     Check whether HandlerTypes is async or not.
    /// </summary>
    public bool IsAsyncType => Type == HandlerTypes.AfterAsync || Type == HandlerTypes.BeforeAsync;
}