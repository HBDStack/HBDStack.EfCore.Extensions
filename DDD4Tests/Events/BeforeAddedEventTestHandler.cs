using System.Threading;
using System.Threading.Tasks;
using DDD4Tests.Domains;
using HBDStack.EfCore.Abstractions.Events;
using HBDStack.EfCore.Events.Handlers;
using HBDStack.StatusGeneric;

namespace DDD4Tests.Events;

internal class BeforeAddedEventTestHandler : /*IBeforeSaveEventHandler<EntityAddedEvent>,*/
    IBeforeSaveEventHandlerAsync<EntityAddedEvent>
{
    public BeforeAddedEventTestHandler()
    {
        Called = false;
        AsyncCalled = false;
    }

    public static bool ReturnFailureResult { get; set; }

    public static bool Called { get; set; }
    public static bool AsyncCalled { get; set; }

    public IStatusGeneric Handle(IEventEntity callingEntity, EntityAddedEvent domainEvent)
    {
        Called = callingEntity != null && domainEvent != null;

        var status = new StatusGenericHandler();
        if (ReturnFailureResult)
            status.AddError("Some Error Here");

        return status;
    }

    public ValueTask<IStatusGeneric> HandleAsync(IEventEntity callingEntity, EntityAddedEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        if (callingEntity is Entity entity)
        {
            entity.Rename("Long");
        }
        
        var status = new StatusGenericHandler();
        if (ReturnFailureResult)
            status.AddError("Some Error Here");

        AsyncCalled = callingEntity != null && domainEvent != null;

        return new ValueTask<IStatusGeneric>( status);
    }
}