using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HBDStack.EfCore.Relational.Helpers.Tests.Data;

[Table(nameof(TestEntity))]
public class TestEntity
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; } = default!;
}

[Table(nameof(NotMappedTestEntity))]
public class NotMappedTestEntity
{
    [Key]
    public int Id { get; set; }
    
    public string Name { get; set; }= default!;
}

public class TestDbContext : DbContext
{
    public TestDbContext(DbContextOptions<TestDbContext>options):base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<TestEntity>();
    }
}