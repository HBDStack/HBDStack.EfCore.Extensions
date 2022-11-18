using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HBDStack.EfCore.Abstractions.Events;
using HBDStack.EfCore.Events.Handlers;
using HBDStack.StatusGeneric;

namespace DDD4Tests.Events;

internal class BeforeUpdatedEventTestHandler : /*IBeforeSaveEventHandler<EntityAddedEvent>,*/
    IBeforeSaveEventHandlerAsync<EntityUpdatedEvent>
{
    public static readonly List<string> ProceedHandlers = new();

    public ValueTask<IStatusGeneric> HandleAsync(IEventEntity callingEntity, EntityUpdatedEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        ProceedHandlers.Add($"{callingEntity.GetType().Name}{domainEvent.Id}");
        return new ValueTask<IStatusGeneric>(new StatusGenericHandler());
    }
}