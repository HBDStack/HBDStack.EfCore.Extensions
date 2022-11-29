using DDD4Tests.Abstracts;
using DDD4Tests.Events;

namespace DDD4Tests.Domains.University;

public class Course : AggregateRoot
{
    public Course(string name) : base(default, "Unit test")
    {
        Name = name;
    }

    public string Name { get; private set; }

    public string Status { get; private set; }

    public void Start()
    {
        Status = "Started";
        AddEvent(new CourseStartedEvent());
        AddEvent(new CourseStatusChangedEvent());
    }
}