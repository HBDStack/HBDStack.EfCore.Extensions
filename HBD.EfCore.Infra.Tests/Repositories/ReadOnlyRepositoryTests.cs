using System.Threading.Tasks;
using DDD4Tests;
using DDD4Tests.Domains;
using FluentAssertions;
using HBD.EfCore.Repos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace HBD.EfCore.Infra.Tests.Repositories;

public class ReadOnlyRepositoryTests : IClassFixture<RepositoryFixture>
{
    private readonly RepositoryFixture _fixture;

    public ReadOnlyRepositoryTests(RepositoryFixture fixture)
    {
        _fixture = fixture;
    }

    #region Methods

    [Fact]
    public async Task ReadAsync()
    {
        var db = _fixture.Provider.GetRequiredService<DddContext>();
        var repo = _fixture.Provider.GetRequiredService<IReadOnlyRepository<Root>>();

        var users = await repo.GetAsync(u => u.Name.Length > 0).ConfigureAwait(false);
        users.Should().HaveCountGreaterOrEqualTo(1);
    }

    [Fact]
    public async Task ReadConditionAsync()
    {
        var db = _fixture.Provider.GetRequiredService<DddContext>();
        var repo = _fixture.Provider.GetRequiredService<IReadOnlyRepository<Root>>();

        var users = await repo.GetAsync(u => u.Name.Length > 3, u => u.Name).ConfigureAwait(false);
        users.Should().HaveCountGreaterOrEqualTo(1);
    }


    [Fact]
    public async Task FindAsync()
    {
        var db = _fixture.Provider.GetRequiredService<DddContext>();
        var repo = _fixture.Provider.GetRequiredService<IReadOnlyRepository<Root>>();

        var p = await db.Set<Root>().AsNoTracking().FirstAsync().ConfigureAwait(false);
        var u = await repo.FindAsync(p.Id).ConfigureAwait(false);
        u.Should().NotBeNull();
    }

    [Fact]
    public async Task FindWithFilterAsync()
    {
        var db = _fixture.Provider.GetRequiredService<DddContext>();
        var repo = _fixture.Provider.GetRequiredService<IReadOnlyRepository<Root>>();

        var p = await db.Set<Root>().AsNoTracking().FirstAsync().ConfigureAwait(false);
        var u = await repo.FindAsync(i => i.Id == p.Id).ConfigureAwait(false);
        u.Should().NotBeNull();
    }

    #endregion Methods
}