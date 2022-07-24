using System;
using AutoMapper;
using DDD4Tests.Domains;
using HBDStack.EfCore.Abstractions.Events;

namespace DDD4Tests.Events;

[AutoMap(typeof(Root))]
public record EntityAddedEvent : IEventItem
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}

[AutoMap(typeof(Root))]
public record EntityUpdatedEvent : IEventItem
{
    public Guid Id { get; set; }

    public string Name { get; set; }
}

[AutoMap(typeof(Root))]
public record EntityDeletedEvent : IEventItem
{
    public Guid Id { get; set; }

    public string Name { get; set; }
}

[AutoMap(typeof(Root))]
public record TypeEvent : IEventItem
{
    public Guid Id { get; set; }

    public string Name { get; set; }
}