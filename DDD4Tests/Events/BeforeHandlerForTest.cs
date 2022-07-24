using System.Threading;
using System.Threading.Tasks;
using HBDStack.EfCore.Abstractions.Events;
using HBDStack.EfCore.Events.Handlers;
using StatusGeneric;

namespace DDD4Tests.Events;

internal class BeforeEventTestHandler : /*IBeforeSaveEventHandler<EntityAddedEvent>,*/
    IBeforeSaveEventHandlerAsync<EntityAddedEvent>
{
    public BeforeEventTestHandler()
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
        var status = new StatusGenericHandler();
        if (ReturnFailureResult)
            status.AddError("Some Error Here");

        AsyncCalled = callingEntity != null && domainEvent != null;

        return new ValueTask<IStatusGeneric>( status);
    }
}