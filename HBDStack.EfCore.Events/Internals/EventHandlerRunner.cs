using HBDStack.EfCore.Abstractions.Events;
using HBDStack.EfCore.Events.Handlers;
using StatusGeneric;

namespace HBDStack.EfCore.Events.Internals;

// internal abstract class EventHandlerRunner
// {
//     public abstract IStatusGeneric Handle(IEventEntity callingEntity, IEventItem domainEvent);
// }

// internal sealed class EventHandlerRunner<TEvent> : EventHandlerRunner where TEvent : IEventItem
// {
//     private readonly HandlerAndEntityEvent _info;
//
//     public EventHandlerRunner(HandlerAndEntityEvent info) => _info = info;
//
//     public override IStatusGeneric Handle(IEventEntity callingEntity, IEventItem domainEvent) 
//         => ((IEventHandler<TEvent>) _info.Handler).Handle(callingEntity, (TEvent) domainEvent);
// }

internal abstract class EventHandlerRunnerAsync
{
    public abstract ValueTask<IStatusGeneric> HandleAsync(IEventEntity callingEntity, IEventItem domainEvent,
        CancellationToken cancellationToken = default);
}

internal sealed class EventHandlerRunnerAsync<TEvent> : EventHandlerRunnerAsync where TEvent : IEventItem
{
    private readonly HandlerAndEntityEvent _info;

    public EventHandlerRunnerAsync(HandlerAndEntityEvent info) => _info = info;

    public override ValueTask<IStatusGeneric> HandleAsync(IEventEntity callingEntity, IEventItem domainEvent,
        CancellationToken cancellationToken = default)
    {
        return ((IEventHandlerAsync<TEvent>) _info.Handler).HandleAsync(callingEntity, (TEvent) domainEvent,
            cancellationToken);
    }
}