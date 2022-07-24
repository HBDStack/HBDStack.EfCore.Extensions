using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace HBDStack.EfCore.AuditTrail.Entities;

[Table("AuditTrails", Schema = AuditTrailModelBuilderRegister.AuditSchema)]
public sealed class DefaultAuditItem : AuditItem
{
    public DefaultAuditItem(string tableName, 
        IDictionary<string, object?> keyValues, 
        IDictionary<string, object?> oldValues, 
        IDictionary<string, object?> newValues, 
        string byUser,
        EntityState action) : base(tableName, keyValues, oldValues, newValues, byUser, action)
    {
        TableName = tableName;
        KeyValues = JsonSerializer.Serialize(keyValues);
        OldValues = JsonSerializer.Serialize(oldValues);
        NewValues = JsonSerializer.Serialize(newValues);
    }

    private DefaultAuditItem()
    {
    }

    [MaxLength(250), Required] public string TableName { get; private set; }= default!;
    [MaxLength(250), Required] public  string KeyValues { get; private set; }= default!;
    [MaxLength(4000)] public  string OldValues { get; private set; }= default!;
    [MaxLength(4000)] public  string NewValues { get; private set; }= default!;
}