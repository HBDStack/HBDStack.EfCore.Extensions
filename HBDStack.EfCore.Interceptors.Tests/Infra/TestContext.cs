using Microsoft.EntityFrameworkCore;

namespace HBDStack.EfCore.Interceptors.Tests.Infra;

public class TestContext : DbContext
{
    public TestContext(DbContextOptions<TestContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<TestTruncateEntity>();
    }
}