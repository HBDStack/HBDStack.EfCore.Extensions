using DDD4Tests.Abstracts;

namespace DDD4Tests.Domains.University;

public class Student : AggregateRoot
{
    public string FullName { get; private set; }

    public Student(string fullName) : base(default, "Unit test")
    {
        FullName = fullName;
    }
}