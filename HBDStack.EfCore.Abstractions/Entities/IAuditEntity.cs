using System.ComponentModel.DataAnnotations;

namespace HBDStack.EfCore.Abstractions.Entities;

public interface IAuditProperties
{
    [Required]
    [MaxLength(255)]
    string CreatedBy { get; }

    [Required]
    DateTimeOffset CreatedOn { get; }
    
    [MaxLength(255)]
    string? UpdatedBy { get; }
    
    DateTimeOffset? UpdatedOn { get; }
}

public interface IAuditEntity<out TKey> : IEntity<TKey>,IAuditProperties, IConcurrencyEntity
{
    void SetCreatedBy(string userName, DateTimeOffset? createdOn = null);
    void SetUpdatedBy(string userName, DateTimeOffset? updatedOn = null);
}