using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
#pragma warning disable CS8618

namespace HBDStack.EfCore.Abstractions.Entities;

[Index(nameof(CreatedBy))]
[Index(nameof(CreatedOn))]
[Index(nameof(UpdatedBy))]
[Index(nameof(UpdatedOn))]
public abstract class AuditEntity<TKey> : Entity<TKey>, IAuditEntity<TKey>
{
    /// <inheritdoc />
    protected AuditEntity(TKey id, string createdBy, DateTimeOffset? createdOn = null) : base(id) =>
        SetCreatedBy(createdBy, createdOn);

    protected AuditEntity()
    {
    }

    [MaxLength(100)]
    [Column(Order = 996)]
    [Required]
    public virtual string CreatedBy { get; private set; }

    [Column(Order = 997)] [Required] public virtual DateTimeOffset CreatedOn { get; private set; }

    [NotMapped] public virtual string LastModifiedBy => UpdatedBy ?? CreatedBy;

    [NotMapped] public virtual DateTimeOffset LastModifiedOn => UpdatedOn ?? CreatedOn;

    [MaxLength(100)]
    [Column(Order = 998)]
    public virtual string? UpdatedBy { get; private set; }

    [Column(Order = 999)] public virtual DateTimeOffset? UpdatedOn { get; private set; }

    // protected virtual bool IsCreatedInfoSet() =>
    //     !(string.IsNullOrEmpty(CreatedBy) || Id == null || Id.Equals(default(TKey)));

    /// <summary>
    /// Use this to update the CreatedBy and CreatedOn
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="createdOn"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public void SetCreatedBy(string userName, DateTimeOffset? createdOn = null)
    {
        if (!string.IsNullOrEmpty(CreatedBy)) return;

        CreatedBy = userName ?? throw new ArgumentNullException(nameof(userName));
        CreatedOn = createdOn ?? DateTimeOffset.Now;
    }

    /// <summary>
    ///  Update UpdatedBy UserName and UpdatedOn info. If CreatedBy is null, then CreatedBy will be set and ignore the UpdatedOn.
    /// Set forceSetUpdatedOn = true to enforce the updated date.
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="updatedOn"></param>
    public void SetUpdatedBy(string userName, DateTimeOffset? updatedOn = null)
    {
        updatedOn ??= DateTimeOffset.Now;

        // if ( !IsCreatedInfoSet())
        // {
        //     SetCreatedBy(userName, updatedOn);
        //     return;
        // }
        
        UpdatedBy = userName ?? throw new ArgumentNullException(nameof(userName));
        UpdatedOn = updatedOn;
    }
}

public abstract class AuditEntityGuid : AuditEntity<Guid>
{
    protected AuditEntityGuid(Guid id, string createdBy, DateTimeOffset? createdOn = null)
        : base(id, createdBy, createdOn)
    {
    }

    /// <inheritdoc />
    protected AuditEntityGuid()
    {
    }
}