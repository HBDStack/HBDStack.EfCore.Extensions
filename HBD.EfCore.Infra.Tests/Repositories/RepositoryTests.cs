using System.Threading.Tasks;
using DDD4Tests;
using DDD4Tests.Domains;
using FluentAssertions;
using HBDStack.EfCore.Repos;
using HBDStack.EfCore.Repos.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace HBD.EfCore.Infra.Tests.Repositories;

public class RepositoryTests : IClassFixture<RepositoryFixture>
{
    private readonly RepositoryFixture _fixture;

    public RepositoryTests(RepositoryFixture fixture) => _fixture = fixture;

    #region Methods

    [Fact]
    public void DbContext_Should_NotBeNull()
    {
        var repo = _fixture.Provider.GetRequiredService<IRepository<Root>>();
        (repo as Repository<Root>)?.DbContext.Should().NotBeNull();
    }

    [Fact]
    public async Task AddAsync()
    {
        var repo = _fixture.Provider.GetRequiredService<IRepository<Root>>();

        var p = new Root("Duy");
        await repo.AddAsync(p).ConfigureAwait(false);
        p.Id.Should().NotBeEmpty();
        
        await repo.SaveAsync().ConfigureAwait(false);
    }
    
    [Fact]
    public async Task AddAsync_OtherEntity_ID_ShouldBe_Defined()
    {
        var repo = _fixture.Provider.GetRequiredService<IRepository<OtherEntity>>();

        var p = new OtherEntity("Duy");
        await repo.AddAsync(p).ConfigureAwait(false);
        
        //NOTE: If ID is int the value will be defined only when saved to the database
        p.Id.Should().BeLessOrEqualTo(0);
        
        await repo.SaveAsync().ConfigureAwait(false);
        p.Id.Should().BePositive();
    }

    [Fact]
    public async Task DeleteByIdAsync()
    {
        var db = _fixture.Provider.GetRequiredService<DddContext>();
        var repo = _fixture.Provider.GetRequiredService<IRepository<Root>>();

        var p = await db.Set<Root>().FirstOrDefaultAsync().ConfigureAwait(false);
        await repo.DeleteByIdAsync(p.Id).ConfigureAwait(false);
        await repo.SaveAsync().ConfigureAwait(false);

        var t = await db.Set<Root>().FirstOrDefaultAsync(i => i.Id == p.Id).ConfigureAwait(false);
        t.Should().BeNull();
    }
        
    [Fact]
    public async Task DeleteAsync()
    {
        var db = _fixture.Provider.GetRequiredService<DddContext>();
        var repo = _fixture.Provider.GetRequiredService<IRepository<Root>>();

        var p = await db.Set<Root>().FirstOrDefaultAsync().ConfigureAwait(false);
        repo.Delete(p);
        await repo.SaveAsync().ConfigureAwait(false);

        var t = await db.Set<Root>().FirstOrDefaultAsync(i => i.Id == p.Id).ConfigureAwait(false);
        t.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync()
    {
        var repo = _fixture.Provider.GetRequiredService<IRepository<Root>>();

        var p = new Root("Duy");
        await repo.AddAsync(p).ConfigureAwait(false);
        await repo.SaveAsync().ConfigureAwait(false);

        p.Id.Should().NotBeEmpty();

        p.UpdateName("Hoang");
        await repo.SaveAsync().ConfigureAwait(false);

        p = await repo.FindAsync(p.Id).ConfigureAwait(false);
        p.Name.Should().Be("Hoang");
    }

    #endregion Methods
}