using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DDD4Tests.Abstracts;
using DDD4Tests.Events;
using HBDStack.EfCore.Events.Attributes;
using Microsoft.EntityFrameworkCore;

namespace DDD4Tests.Domains;

[AutoEvents(CreatedEventType = typeof(EntityAddedEvent),UpdatedEventType = typeof(EntityUpdatedEvent),DeletedEventType = typeof(EntityDeletedEvent))]
public class Root : AggregateRoot
{
    private readonly HashSet<Entity> _entities;

    public Root(string name) : base(default, $"Unit Test {Guid.NewGuid()}")
    {
        Name = name;
        _entities = new HashSet<Entity>();
    }

    [Required] public string Name { get; private set; }
        
    [BackingField(nameof(_entities))]
    public IReadOnlyCollection<Entity> Entities => _entities;

    public void UpdateName(string name) => Name = name;

    public void AddEntity(string name)
    {
        var entity = new Entity(name, Id);
        _entities.Add(entity);
    }
}