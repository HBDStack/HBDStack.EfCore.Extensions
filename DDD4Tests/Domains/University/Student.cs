using System;
using DDD4Tests.Abstracts;
using DDD4Tests.Events;

namespace DDD4Tests.Domains.University;

public class Student : AggregateRoot
{
    public Student(Guid id, string fullName, Guid courseId) : base(id, "Unit test")
    {
        FullName = fullName;
        CourseId = courseId;
        AddEvent(new StudentAddedEvent());
    }

    public string FullName { get; private set; }

    public Guid CourseId { get; private set; }
}