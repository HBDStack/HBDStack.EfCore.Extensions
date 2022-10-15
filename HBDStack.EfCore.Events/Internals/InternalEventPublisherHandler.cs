using HBDStack.EfCore.Abstractions.Events;
using HBDStack.EfCore.Events.Handlers;
using HBDStack.StatusGeneric;

namespace HBDStack.EfCore.Events.Internals;

internal sealed class InternalEventPublisherHandler<TEvent> : /*IAfterSaveEventHandler<TEvent>,*/
    IAfterSaveEventHandlerAsync<TEvent> where TEvent : IEventItem
{
    private readonly IEventPublisher _eventPublisher;
    public InternalEventPublisherHandler(IEventPublisher eventPublisher) => _eventPublisher = eventPublisher;

    public IStatusGeneric Handle(IEventEntity callingEntity, TEvent domainEvent)
    {
        //Trace.TraceInformation($"AfterSaveEventPublisher ${nameof(Handle)}: {domainEvent.GetType().Name}");

        _eventPublisher.PublishAsync(domainEvent).ConfigureAwait(false).GetAwaiter().GetResult();
        return new StatusGenericHandler();
    }

    public async ValueTask<IStatusGeneric> HandleAsync(IEventEntity callingEntity, TEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        //Trace.TraceInformation($"AfterSaveEventPublisher ${nameof(HandleAsync)}: {domainEvent.GetType().Name}");

        await _eventPublisher.PublishAsync(domainEvent).ConfigureAwait(false);
        return new StatusGenericHandler();
    }
}