using System.Reflection;
using HBDStack.EfCore.Extensions.Services;
using HBDStack.EfCore.AuditTrail.Attributes;
using HBDStack.EfCore.AuditTrail.Entities;
using Microsoft.EntityFrameworkCore;

namespace HBDStack.EfCore.AuditTrail;

internal class AuditTrailModelBuilderRegister : IGlobalModelBuilderRegister
{
    internal static readonly Type DefaultAuditItemType = typeof(DefaultAuditItem);
    internal const string AuditSchema = "audit";
    
    public void Apply(ModelBuilder modelBuilder, DbContext context)
    {
        var auditItemTypes = modelBuilder.Model.GetEntityTypes()
            .Select(m => m.ClrType.GetCustomAttribute<AuditTrailAttribute>())
            .Where(a => a != null && a.AuditTrailItemType != DefaultAuditItemType)
            .Select(a => a!.AuditTrailItemType)
            .Distinct()
            .ToList();

        foreach (var type in auditItemTypes)
        {
            //var att = type.GetCustomAttribute<TableAttribute>();
            modelBuilder.Entity(type);//.ToTable(att?.Name??type.Name, att?.Schema??AuditSchema);
        }
        
        //Add default Types
        modelBuilder.Entity(DefaultAuditItemType);//.ToTable("AuditTrails",AuditSchema);
    }
}