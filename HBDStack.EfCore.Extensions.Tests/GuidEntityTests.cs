using System.Threading.Tasks;
using DataLayer;
using FluentAssertions;
using HBDStack.TestHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HBDStack.EfCore.Extensions.Tests;

[TestClass]
public class GuidEntityTests
{
    #region Methods

    [TestMethod]
    public async Task TestCreateAsync()
    {
        var entity = new GuidEntity {Name = "Duy"};

        UnitTestSetup.Db.Add(entity);
        await UnitTestSetup.Db.SaveChangesAsync().ConfigureAwait(false);

        entity.Id.Should().NotBeEmpty();
    }

    [TestMethod]
    public async Task TestCreateAuditAsync()
    {
        var entity = new GuidAuditEntity {Name = "Duy"};

        UnitTestSetup.Db.Add(entity);
        await UnitTestSetup.Db.SaveChangesAsync().ConfigureAwait(false);

        entity.Id.Should().NotBeEmpty();
    }

    [TestMethod]
    public async Task TestUpdateAsync()
    {
        var entity = new GuidEntity {Name = "Duy"};
        var oldId = entity.Id.ToString();

        entity.Name = "Hoang";

        await UnitTestSetup.Db.SaveChangesAsync().ConfigureAwait(false);

        entity.Id.ToString().Should().Be(oldId);
    }

    [TestMethod]
    public async Task TestUpdateAuditAsync()
    {
        var entity = new GuidAuditEntity {Name = "Duy"};
        var oldId = entity.Id.ToString();

        entity.Name = "Hoang";

        await UnitTestSetup.Db.SaveChangesAsync().ConfigureAwait(false);

        entity.Id.ToString().Should().Be(oldId);
    }

    #endregion Methods
}