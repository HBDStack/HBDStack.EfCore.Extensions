using System;
using System.Threading;
using System.Threading.Tasks;
using DDD4Tests.Domains.University;
using HBDStack.EfCore.Abstractions.Events;
using HBDStack.EfCore.Events.Handlers;
using HBDStack.StatusGeneric;

namespace DDD4Tests.Events;

public class BeforeCourseAddedEventHandler : IBeforeSaveEventHandlerAsync<CourseAddedEvent>
{
    private readonly DddContext _context;

    public BeforeCourseAddedEventHandler(DddContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async ValueTask<IStatusGeneric> HandleAsync(IEventEntity callingEntity, CourseAddedEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        await _context.AddAsync(new Student("Long Tang"), cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return await new ValueTask<IStatusGeneric>(new StatusGenericHandler());
    }
}