using Microsoft.EntityFrameworkCore;

namespace HBDStack.EfCore.AuditTrail.Tests.Data;

public class TestDbContext:DbContext
{
    public TestDbContext(DbContextOptions<TestDbContext> options):base(options)
    {
    }
    
    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     base.OnModelCreating(modelBuilder);
    //     
    //     modelBuilder.Entity<AuditEntity1>();
    //     modelBuilder.Entity<AuditEntity2>();
    //     modelBuilder.Entity<Entity>();
    // }
}