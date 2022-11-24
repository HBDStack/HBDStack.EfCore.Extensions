using DDD4Tests.Abstracts;
using DDD4Tests.Events;

namespace DDD4Tests.Domains.University;

public class Course : AggregateRoot
{
    public string Name { get; private set; }

    public Course(string name) : base(default, "Unit test")
    {
        Name = name;
        AddEvent(new CourseAddedEvent());
    }
}