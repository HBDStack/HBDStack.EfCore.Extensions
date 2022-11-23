using System.Threading;
using System.Threading.Tasks;
using HBDStack.EfCore.Abstractions.Events;
using HBDStack.EfCore.Events.Handlers;
using HBDStack.StatusGeneric;

namespace DDD4Tests.Events;

public class BeforeStudentAddedEventHandler : IBeforeSaveEventHandlerAsync<StudentAddedEvent>
{
    public ValueTask<IStatusGeneric> HandleAsync(IEventEntity callingEntity, StudentAddedEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        throw new System.NotImplementedException();
    }
}