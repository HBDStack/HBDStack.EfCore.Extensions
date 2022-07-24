using System.Linq;
using DataLayer;
using FluentAssertions;
using HBDStack.TestHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HBDStack.EfCore.Extensions.Tests;

[TestClass]
public class UserTests
{
    #region Methods

    [TestMethod]
    public void AddUserAndAddress()
    {
        var user = new User("A")
        {
            FirstName = "Duy",
            LastName = "Hoang",
            Addresses =
            {
                new Address
                {
                    OwnedEntity = new OwnedEntity
                    {
                        Name = "A"
                    },
                    Street = "123"
                },
                new Address
                {
                    OwnedEntity = new OwnedEntity
                    {
                        Name = "B"
                    },
                    Street = "124"
                }
            }
        };

        UnitTestSetup.Db.Add(user);
        UnitTestSetup.Db.SaveChanges();

        var u = UnitTestSetup.Db.Set<User>().Include(i => i.Addresses).First();
        u.Should().NotBeNull();
        u.Addresses.Should().HaveCountGreaterOrEqualTo(2);

        u.Addresses.Remove(u.Addresses.First());
        UnitTestSetup.Db.SaveChanges();

        u = UnitTestSetup.Db.Set<User>().First();
        u.Addresses.Should().HaveCountGreaterOrEqualTo(1);

        UnitTestSetup.Db.ChangeTracker.AutoDetectChangesEnabled.Should().BeTrue();
    }

    [TestMethod]
    public void Created_User_Id_ShouldBe_Zero()
    {
        var user = new User();
        user.Id.Should().Be(0);
    }

    #endregion Methods
}