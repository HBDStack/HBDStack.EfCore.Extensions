using System;

namespace DDD4Tests.Abstracts;

public abstract class AggregateRoot : EntityBase<Guid>
{
    protected AggregateRoot(string createdBy, DateTimeOffset? createdOn = null)
        : this(default, createdBy, createdOn)
    {
    }

    protected AggregateRoot(Guid id, string createdBy, DateTimeOffset? createdOn = null)
        : base(id, createdBy, createdOn)
    {
    }

    /// <inheritdoc />
    protected AggregateRoot()
    {
    }
}