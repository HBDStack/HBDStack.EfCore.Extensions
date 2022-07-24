using HBDStack.EfCore.Abstractions.Entities;

namespace DataLayer;

public class GuidEntity : EntityGuid
{
    public string Name { get; set; }
}

public class GuidAuditEntity : AuditEntityGuid
{
    public GuidAuditEntity() : base(default, "Unit Test") => Name = "Testing";

    public string Name { get; set; }
}