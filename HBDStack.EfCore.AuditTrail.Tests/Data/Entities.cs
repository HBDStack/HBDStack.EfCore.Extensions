using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HBDStack.EfCore.Abstractions.Entities;
using HBDStack.EfCore.AuditTrail.Attributes;
using HBDStack.EfCore.AuditTrail.Entities;
using Microsoft.EntityFrameworkCore;
#pragma warning disable CS8618

namespace HBDStack.EfCore.AuditTrail.Tests.Data;

[AuditTrail]
public class AuditEntity1 : AuditEntity<int>
{
    public AuditEntity1(string name) : base(default, "UnitTest")
    {
        Name = name;
    }

    private AuditEntity1()
    {
    }

    public string Name { get; private set; }
}

[AuditTrail(typeof(CustomAuditItem), IgnoredProperties = new []{nameof(CreatedBy),nameof(CreatedOn),nameof(UpdatedBy),nameof(UpdatedOn)})]
public class AuditEntity2 : AuditEntity<int>
{
    public AuditEntity2(string name) : base(default, "UnitTest")
    {
        Name = name;
    }

    private AuditEntity2()
    {
    }

    public string Name { get; private set; }
}

[Table("CustomAuditTrails", Schema = "audit")]
public sealed class CustomAuditItem : AuditItem
{
    public CustomAuditItem(string tableName, 
        IDictionary<string, object> keyValues, 
        IDictionary<string, object> oldValues, 
        IDictionary<string, object> newValues, 
        string byUser, 
        EntityState action) : base(tableName, keyValues, oldValues, newValues, byUser, action)
    {
        Message = $"{byUser} had been {action} the {tableName} with at {DateTime}";
    }
    
    private CustomAuditItem()
    {
    }
    
    [MaxLength(500)]
    public string Message { get; private set; }
}

public class Entity : AuditEntity<int>
{
    public Entity(string name) : base(default, "UnitTest")
    {
        Name = name;
    }

    private Entity()
    {
    }

    public string Name { get; private set; }
}