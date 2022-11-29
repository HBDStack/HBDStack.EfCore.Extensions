using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HBDStack.EfCore.Abstractions.Events;
using HBDStack.EfCore.Events.Handlers;
using HBDStack.StatusGeneric;

namespace DDD4Tests.Events;

internal class BeforeCourseStartedEventHandler : IBeforeSaveEventHandlerAsync<CourseStartedEvent>
{
    public static readonly List<string> ProceedHandlers = new();

    public async ValueTask<IStatusGeneric> HandleAsync(IEventEntity callingEntity, CourseStartedEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        ProceedHandlers.Add($"{domainEvent.GetType().Name}");
        return await new ValueTask<IStatusGeneric>(new StatusGenericHandler());
    }
}