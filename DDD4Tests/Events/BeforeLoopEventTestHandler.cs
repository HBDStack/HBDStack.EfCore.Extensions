using System.Threading;
using System.Threading.Tasks;
using DDD4Tests.Domains;
using HBDStack.EfCore.Abstractions.Events;
using HBDStack.EfCore.Events.Handlers;
using HBDStack.StatusGeneric;

namespace DDD4Tests.Events;

internal class BeforeLoopEventTestHandler : /*IBeforeSaveEventHandler<EntityLoopEvent>,*/
    IBeforeSaveEventHandlerAsync<EntityLoopEvent>
{
    public ValueTask<IStatusGeneric> HandleAsync(IEventEntity callingEntity, EntityLoopEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        var entity = callingEntity as Entity;
        entity?.DoLoop();

        return new ValueTask<IStatusGeneric>(new StatusGenericHandler());
    }
}