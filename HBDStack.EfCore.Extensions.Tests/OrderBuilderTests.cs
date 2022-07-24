using System.Threading.Tasks;
using DataLayer;
using FluentAssertions;
using HBDStack.EfCore.Abstractions.QueryBuilders;
using HBDStack.TestHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HBDStack.EfCore.Extensions.Tests;

[TestClass]
public class OrderBuilderTests
{
    #region Methods

    [TestMethod]
    public async Task TestOrderBuilder_User_ByProps()
    {
        await UnitTestSetup.Db.SeedData().ConfigureAwait(false);

        var orderBuilder = OrderBuilder.CreateBuilder<User>()
            .OrderBy(u => u.CreatedBy)
            .ThenBy(u => u.FirstName);

        var list = await orderBuilder.Build(UnitTestSetup.Db.Set<User>()).ToListAsync().ConfigureAwait(false);

        list.Should().NotBeEmpty();
    }

    [TestMethod]
    public async Task TestOrderBuilder_User_ByProps_Desc()
    {
        await UnitTestSetup.Db.SeedData().ConfigureAwait(false);

        var orderBuilder = OrderBuilder.CreateBuilder<User>()
            .OrderByDescending(u => u.CreatedBy)
            .ThenByDescending(u => u.FirstName);

        var list = await orderBuilder.Build(UnitTestSetup.Db.Set<User>()).ToListAsync().ConfigureAwait(false);

        list.Should().NotBeEmpty();
    }

    [TestMethod]
    public async Task TestOrderBuilder_User_ByPropsString()
    {
        await UnitTestSetup.Db.SeedData().ConfigureAwait(false);

        var orderBuilder = OrderBuilder.CreateBuilder<User>()
            .OrderBy(nameof(User.CreatedBy))
            .ThenBy(nameof(User.FirstName));

        var list = await orderBuilder.Build(UnitTestSetup.Db.Set<User>()).ToListAsync().ConfigureAwait(false);

        list.Should().NotBeEmpty();
    }

    [TestMethod]
    public async Task TestOrderBuilder_User_ByPropsString_Desc()
    {
        await UnitTestSetup.Db.SeedData().ConfigureAwait(false);

        var orderBuilder = OrderBuilder.CreateBuilder<User>()
            .OrderByDescending(nameof(User.CreatedBy))
            .ThenByDescending(nameof(User.FirstName));

        var list = await orderBuilder.Build(UnitTestSetup.Db.Set<User>()).ToListAsync().ConfigureAwait(false);

        list.Should().NotBeEmpty();
    }

    [TestMethod]
    public async Task TestOrderBuilder_User_OrderWith()
    {
        await UnitTestSetup.Db.SeedData().ConfigureAwait(false);

        var orderBuilder = OrderBuilder.CreateBuilder<User>()
            .OrderByDescending(u => u.LastName)
            .ThenByDescending(nameof(User.FirstName));

        var list = await UnitTestSetup.Db.Set<User>().OrderWith(orderBuilder).ToListAsync().ConfigureAwait(false);

        list.Should().NotBeEmpty();
    }

    #endregion Methods
}