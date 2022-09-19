using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HBDStack.EfCore.Abstractions.Entities;

/// <summary>
///     Abstract Entity with Key definition parameter.
/// </summary>
/// <typeparam name="TKey"></typeparam>
public abstract class Entity<TKey> : IEntity<TKey>, IConcurrencyEntity
{
    protected Entity()
    {
    }

    /// <summary>
    ///     Constructor for EF Core using for Data Seeding
    /// </summary>
    protected Entity(TKey id) => Id = id;

    [Key]
    [Column(Order = 0)]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //[IgnoreFromUpdate]
    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local
    public virtual TKey Id { get; private set; } = default!;

    /// <summary>
    ///     The RowVersion which using by EF
    /// </summary>

    [Column(Order = 1000)]
    [Timestamp, ConcurrencyCheck]
    public virtual byte[]? RowVersion { get; private set; }

    public void SetRowVersion(byte[] rowVersion) => RowVersion = rowVersion;

    public override string ToString() => $"{GetType().Name} '{Id}'";
}

/// <summary>
///     The Base Entity with Id is Guid
/// </summary>
public abstract class EntityGuid : Entity<Guid>
{
    /// <inheritdoc />
    /// <summary>
    ///     Constructor for EF Core
    /// </summary>
    protected EntityGuid()
    {
    }

    /// <inheritdoc />
    /// <summary>
    ///     Constructor for EF Core using for Data Seeding
    /// </summary>
    protected EntityGuid(Guid id) : base(id)
    {
    }
}