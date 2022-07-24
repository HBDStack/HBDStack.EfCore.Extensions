# HBD.EfCore.AuditTrail
The audit trail service for EntityFramework Core (EfCore). HBD.EfCore.AuditTrail is a service that is used to log changes of the entities into AutiTrail tables.
This service is simplifying the audit trail implementation also provide flexibility to customize the audit message.

## How to use HBD.EfCore.AuditTrail?
The HBD.EfCore.AuditTrail had been built on top of **HBD.EfCore.Hooks** for audit trail purposes. The default audit trail information will be store in the audit.AuditTrails table.

1. Download HBD.EfCore.AuditTrail from NuGet.
```shell
$ dotnet add package HBD.EfCore.AuditTrail
```

2. Add the HBD.EfCore.AuditTrail to the services into ServiceCollection. Please ensure the HBD.EfCore.Hooks is enabled for your DbContext.
```csharp
ServiceProvider = new ServiceCollection()
    .AddLogging()
    .AddDbContextWithHook<TestDbContext>(op=>op //HBD.EfCore.Hooks
        .UseAutoConfigModel() //HBD.EfCore.Extentions
        .UseSqlServer("Your Database Connection String"))
    .AddAuditTrailServices() //HBD.EfCore.AuditTrail
    .BuildServiceProvider();
```
3. Configure audit trail for Entities with **AuditTrailAttribute**. Please note that your entity must provide all the properties required from *IAuditProperties*
```csharp
[AuditTrail]
public class AuditEntity1 : IAuditProperties
{
}
```
That's all. Whenever the AuditEntity1 got created or updated the audit trail info will be stored in the audit.AuditTrails table.

## How to ignore some properties from audit trail?
The AuditTrailAttribute allows to ignore some properties from audit trail with following syntax.
```csharp
[AuditTrail(IgnoredProperties = new []{"Array of Property's Name"})]
public class AuditEntity2 : IAuditProperties
{
   
}
```

## How to separate audit trail to other table?
First, you need to define a new audit trail entity that MUST be inherited from AuditItem as below sample.
```csharp
[Table("CustomAuditTrails",Schema = "audit")]
public sealed class CustomAuditItem : AuditItem
{
    public CustomAuditItem(string tableName, 
        IDictionary<string, object?> keyValues, 
        IDictionary<string, object?> oldValues, 
        IDictionary<string, object?> newValues, 
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
```

Second, update the AuditTrailAttribute to use the new audit trail entity.
```csharp
[AuditTrail(typeof(CustomAuditItem))]
public class AuditEntity2 : IAuditProperties
{
   
}
```
That's it. The audit trail info of **AuditEntity2** will be stored in the **CustomAuditTrails** table.