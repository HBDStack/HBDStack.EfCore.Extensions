using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace HBDStack.EfCore.AuditTrail.Entities;

public abstract class AuditItem
{
    public AuditItem(string tableName, 
        IDictionary<string, object?> keyValues, 
        IDictionary<string, object?> oldValues, 
        IDictionary<string, object?> newValues, 
        string byUser,
        EntityState action)
    {
        ByUser = byUser;
        Action = action;
        DateTime = DateTimeOffset.Now;
    }

    protected AuditItem()
    {
    }

    [Key] public virtual long Id { get; private set; } = default!;

    [MaxLength(500), Required] public virtual string ByUser { get; private set; } = default!;

    [MaxLength(50), Required] public virtual EntityState Action { get; private set; }= default!;
    
    public virtual DateTimeOffset DateTime { get; private set; }= default!;
}