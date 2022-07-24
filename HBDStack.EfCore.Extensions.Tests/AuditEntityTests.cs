using System.Threading.Tasks;
using DataLayer;
using FluentAssertions;
using HBDStack.TestHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HBDStack.EfCore.Extensions.Tests;

[TestClass]
public class AuditEntityTests
{
    #region Methods

    [TestMethod]
    public void TestCreatingEntity()
    {
        var user = new User("Duy");
        user.UpdatedByUser("Hoang");

        //TODO allow updatedBy the same with createdBy
        //user.UpdatedBy.Should().BeNullOrEmpty();
        //user.UpdatedOn.Should().BeNull();
        
        user.Id.Should().Be(0);
    }

    [TestMethod]
    public async Task TestUpdatingEntityAsync()
    {
        await UnitTestSetup.Db.SeedData().ConfigureAwait(false);
        UnitTestSetup.Db.ChangeTracker.Clear();

        var user = await UnitTestSetup.Db.Set<User>().FirstAsync();
        user.Should().NotBeNull();

        user.UpdatedByUser("Hoang");

        user.UpdatedBy.Should().Be("Hoang");
        user.UpdatedOn.Should().NotBeNull();
        user.Id.Should().Be(1);
    }

    #endregion Methods
}