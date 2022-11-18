using System.Threading;
using System.Threading.Tasks;
using HBDStack.EfCore.Abstractions.Events;
using HBDStack.EfCore.Events.Handlers;
using HBDStack.StatusGeneric;

namespace DDD4Tests.Events;

internal sealed class AfterAddedEventTestHandler : IAfterSaveEventHandlerAsync<EntityAddedEvent>
{
    public AfterAddedEventTestHandler() => AsyncCalled = 0;

    public static bool ReturnFailureResult { get; set; }
    public static int AsyncCalled { get; set; }
        
    public ValueTask<IStatusGeneric> HandleAsync(IEventEntity callingEntity, EntityAddedEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        var status = new StatusGenericHandler();
            
        if (ReturnFailureResult)
            status.AddError("Some Error Here");

        AsyncCalled += 1;

        return new ValueTask<IStatusGeneric>( status);
    }
}