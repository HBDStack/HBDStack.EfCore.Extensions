using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using HBDStack.EfCore.Abstractions.Entities;
using HBDStack.EfCore.Abstractions.Events;
using HBDStack.EfCore.DataAuthorization;

namespace DDD4Tests.Abstracts;

public abstract class EntityBase<TKey> : AuditEntity<TKey>, IEventEntity, IDataKeyEntity
{
    [NotMapped] private readonly ICollection<IEventItem> _events = new Collection<IEventItem>();
    [NotMapped] private readonly ICollection<Type> _eventTypes = new Collection<Type>();

    /// <inheritdoc />
    protected EntityBase(TKey id, string createdBy, DateTimeOffset? createdOn = null) : base(id, createdBy,
        createdOn)
    {
    }

    /// <inheritdoc />
    protected EntityBase()
    {
    }

    public Guid? DataKey { get; private set; }

    public void AddEvent(IEventItem @event) => _events.Add(@event);
    public void AddEvent<TEvent>() where TEvent : IEventItem => _eventTypes.Add(typeof(TEvent));

    public (IEventItem[]events, Type[]eventTypes) GetEventsAndClear()
    {
        var events = _events.ToArray();
        var eventTypes = _eventTypes.ToArray();
        _events.Clear();
        _eventTypes.Clear();

        return (events, eventTypes);
    }

    public void UpdateDataKey(Guid dataKey) => DataKey = dataKey;

    public override string ToString() => $"{GetType().Name} '{Id}'";
}