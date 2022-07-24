using System;
using DDD4Tests.Abstracts;

namespace DDD4Tests.Domains;

public class Entity : DomainEntity
{
    public Entity() : base(default,"Unit Test")
    {
    }

    public Entity(string name, Guid rootId) : base(default, "Unit Test")
    {
        Name = name;
        RootId = rootId;
    }

    public string Name { get; }

    public Guid RootId { get; }

    public Root Parent { get; private set; }
}