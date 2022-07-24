using System;
using System.Linq;
using System.Threading.Tasks;
using AutoBogus;
using DDD4Tests;
using DDD4Tests.Domains;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace HBDStack.EfCore.DataAuthorization.Tests;

public class DataKeyTests : IClassFixture<DataKeyFixture>
{
    private readonly DataKeyFixture _fixture;

    public DataKeyTests(DataKeyFixture fixture)
    {
        _fixture = fixture;
    }

    #region Methods

    [Fact]
    public void TestContextDataKeys()
    {
        //Create account for Key
        var db = _fixture.Provider.GetRequiredService<DddContext>();
        db.OwnershipKey.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public async Task TestDataKeys()
    {
        //Create account for Key
        var db = _fixture.Provider.GetRequiredService<DddContext>();
        var accounts1 = AutoFaker.Generate<Root>(100);

        await db.AddRangeAsync(accounts1).ConfigureAwait(false);
        await db.SaveChangesAsync().ConfigureAwait(false);

        //Verify Key
        db.Set<Root>().ToList()
            .All(a => ((IDataKeyEntity) a).DataKey == db.OwnershipKey)
            .Should().BeTrue();
    }

    [Fact]
    public void TestDataKeysSetup()
    {
        //Create account for Key
        var db = _fixture.Provider.GetRequiredService<DddContext>();
        db.Database.EnsureCreated();

        var etype = db.Model.GetEntityTypes().FirstOrDefault(e => e.ClrType == typeof(Root));
        etype.Should().NotBeNull();
        etype.GetQueryFilter().Should().NotBeNull();
    }

    #endregion Methods
}