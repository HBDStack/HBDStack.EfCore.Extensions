using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DDD4Tests.Domains.University;
using HBDStack.EfCore.Abstractions.Events;
using HBDStack.EfCore.Events.Handlers;
using HBDStack.StatusGeneric;
using Microsoft.EntityFrameworkCore;

namespace DDD4Tests.Events;

internal class BeforeStudentAddedEventHandler : IBeforeSaveEventHandlerAsync<StudentAddedEvent>
{
    private readonly DddContext _context;
    public static readonly List<string> ProceedHandlers = new();

    public BeforeStudentAddedEventHandler(DddContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async ValueTask<IStatusGeneric> HandleAsync(IEventEntity callingEntity, StudentAddedEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        var student = callingEntity as Student;
        var course = await _context.Set<Course>()
            .FirstOrDefaultAsync(i => i.Id == student.CourseId, cancellationToken);
        if (course == null || student == null)
            throw new AggregateException("Course or Student is null");

        course.Start();

        ProceedHandlers.Add($"{domainEvent.GetType().Name}{student.Id}");
        return await new ValueTask<IStatusGeneric>(new StatusGenericHandler());
    }
}