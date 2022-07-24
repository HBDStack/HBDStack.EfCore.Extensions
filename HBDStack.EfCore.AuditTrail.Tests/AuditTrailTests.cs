using System.Threading.Tasks;
using FluentAssertions;
using HBDStack.EfCore.AuditTrail.Entities;
using HBDStack.EfCore.AuditTrail.Tests.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace HBDStack.EfCore.AuditTrail.Tests;

public class AuditTrailTests:IClassFixture<AuditTrailFixture>
{
    private readonly IServiceScope _serviceProvider;

    public AuditTrailTests(AuditTrailFixture fixture) =>
        _serviceProvider = fixture.ServiceProvider.CreateScope();

    [Fact]
    public async Task Test_AuditEntity1()
    {
        var db = _serviceProvider.ServiceProvider.GetRequiredService<TestDbContext>();
        await db.Database.EnsureCreatedAsync().ConfigureAwait(false);
        
        await db.AddAsync(new AuditEntity1("Hello")).ConfigureAwait(false);

        await db.SaveChangesAsync().ConfigureAwait(false);
        
        (await db.Set<DefaultAuditItem>().CountAsync().ConfigureAwait(false))
            .Should().BeGreaterOrEqualTo(1);
    }
    
    [Fact]
    public async Task Test_AuditEntity2()
    {
        var db = _serviceProvider.ServiceProvider.GetRequiredService<TestDbContext>();
        await db.Database.EnsureCreatedAsync().ConfigureAwait(false);
        
        await db.AddAsync(new AuditEntity2("Hello")).ConfigureAwait(false);

        await db.SaveChangesAsync().ConfigureAwait(false);
        
        (await db.Set<CustomAuditItem>().CountAsync().ConfigureAwait(false))
            .Should().BeGreaterOrEqualTo(1);
    }
}