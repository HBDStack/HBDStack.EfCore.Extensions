using System.Linq;
using System.Threading.Tasks;
using DDD4Tests;
using DDD4Tests.Domains;
using DDD4Tests.Dtos;
using FluentAssertions;
using HBDStack.EfCore.Abstractions.QueryBuilders;
using HBDStack.EfCore.Repos.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace HBD.EfCore.Infra.Tests.Repositories;

public class ReadOnlyDtoRepositoryTests : IClassFixture<RepositoryFixture>
{
    private readonly RepositoryFixture _fixture;

    public ReadOnlyDtoRepositoryTests(RepositoryFixture fixture)
    {
        _fixture = fixture;
    }

    #region Methods

    [Fact]
    public async Task ReadAsync()
    {
        var db = _fixture.Provider.GetRequiredService<DddContext>();
        var repo = _fixture.Provider.GetRequiredService<IDtoRepository<Root>>();

        var rs = await repo.Get<RootDto>().ToListAsync().ConfigureAwait(false);
        rs.Should().HaveCountGreaterOrEqualTo(1);
        rs.All(i => i.Name != null).Should().BeTrue();
    }

    [Fact]
    public async Task ReadPageAsync()
    {
        var db = _fixture.Provider.GetRequiredService<DddContext>();
        var repo = _fixture.Provider.GetRequiredService<IDtoRepository<Root>>();

        var rs = await repo.PageAsync(1, 10, OrderBuilder.CreateBuilder<RootDto>(i => i.Id)).ConfigureAwait(false);

        rs.Items.Should().HaveCountGreaterOrEqualTo(1);
    }

    [Fact]
    public async Task ReadSingleAsync()
    {
        var db = _fixture.Provider.GetRequiredService<DddContext>();
        var repo = _fixture.Provider.GetRequiredService<IDtoRepository<Root>>();

        var p = db.Set<Root>().First();
        var rs = await repo.FindAsync<RootDto>(i => i.Id == p.Id).ConfigureAwait(false);

        rs.Should().NotBeNull();
        rs.Id.Should().Be(p.Id);
    }

    #endregion Methods
}