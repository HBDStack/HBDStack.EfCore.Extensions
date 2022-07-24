using System.Linq;
using DataLayer;
using FluentAssertions;
using HBDStack.TestHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HBDStack.EfCore.Extensions.Tests;

[TestClass]
public class ExtensionsTests
{
    #region Methods

    [TestMethod]
    public void Test_GetKeys()
    {
        UnitTestSetup.Db.GetKeys<User>().Single()
            .Should().Be("Id");
    }

    // [TestMethod]
    // public void Test_GetKeys_NotEntity()
    // {
    //     UnitTestSetup.Db.GetKeys<UserAccountStartWithDSpec>().Any()
    //         .Should().BeFalse();
    // }

    [TestMethod]
    public void Test_GetKeyValue()
    {
        var user = new User(1, "Duy");
        UnitTestSetup.Db.GetKeyValuesOf<User>(user).Single()
            .Should().Be(1);
    }

    [TestMethod]
    public void Test_GetKeyValue_NotEntity()
    {
        var user = new {Id = 1, Name = "Duy"};
        UnitTestSetup.Db.GetKeyValuesOf<object>(user).Any()
            .Should().BeFalse();
    }

    ///NOTE as Ef5 and Ef6 already fixed owned property issues. So this method is no longer needed
    // [TestMethod]
    // [ExpectedException(typeof(ArgumentNullException))]
    // public void TestNUll_UpdateFrom()
    // {
    //     ((object) null).UpdateFrom("A");
    // }
    //
    // [TestMethod]
    // public void TestUpdateFrom()
    // {
    //     var user = new OwnedEntity {Name = "Duy"};
    //     var user1 = new OwnedEntity {Name = "Steven"};
    //
    //     user.UpdateFrom(user1);
    //
    //     user.Name.Should().Be(user1.Name);
    // }
    //
    // [TestMethod]
    // public void TestUpdateFrom_IgnoreKey()
    // {
    //     var user = new OwnedEntity {Id = 1, Name = "Duy"};
    //     var user1 = new OwnedEntity {Id = 2, Name = null};
    //
    //     user.UpdateFrom(user1, ignoreKey: true);
    //     user.Id.Should().Be(1);
    // }
    //
    // [TestMethod]
    // public void TestUpdateFrom_IgnoreNull()
    // {
    //     var user = new OwnedEntity {Name = "Duy"};
    //     var user1 = new OwnedEntity {Name = null};
    //
    //     user.UpdateFrom(user1, true);
    //     user.Name.Should().Be("Duy");
    // }
    //
    // [TestMethod]
    // public void TestUpdateFrom_IncludesKey()
    // {
    //     var user = new OwnedEntity {Id = 1, Name = "Duy"};
    //     var user1 = new OwnedEntity {Id = 2, Name = null};
    //
    //     user.UpdateFrom(user1, ignoreKey: false);
    //     user.Id.Should().Be(2);
    // }
    //
    // [TestMethod]
    // [ExpectedException(typeof(ArgumentException))]
    // public void TestUpdateFrom_NotOwnedType()
    // {
    //     var user = new User("Duy");
    //     var user1 = new User("Steven")
    //     {
    //         FirstName = "Duy",
    //         LastName = "Hoang",
    //         Account = new Account()
    //     };
    //
    //     user.UpdateFrom(user1);
    // }
    //
    // [TestMethod]
    // [ExpectedException(typeof(ArgumentNullException))]
    // public void TestUpdateFrom_Null()
    // {
    //     var user = new User("Duy");
    //     user.UpdateFrom(null);
    // }
    //
    // [TestMethod]
    // public void TestUpdateFrom_ReadOnly()
    // {
    //     var user = new OwnedEntity {Name = "Duy", ReadOnly = "A", NotReadOnly = "B"};
    //     var user1 = new OwnedEntity {Name = "Steven", ReadOnly = "B", NotReadOnly = "C"};
    //
    //     user.UpdateFrom(user1);
    //
    //     user.Name.Should().Be(user1.Name);
    //     user.ReadOnly.Should().Be("A");
    //     user.NotReadOnly.Should().Be(user1.NotReadOnly);
    // }
    //
    // [TestMethod]
    // public void TestUpdateFromInternalProps()
    // {
    //     var user = new OwnedEntity {Name = "Duy"};
    //     var user1 = new OwnedEntity("Duy", "Hoang") {Name = "Steven"};
    //
    //     user.UpdateFrom(user1);
    //
    //     user.Name.Should().Be(user1.Name);
    //     user.InternalProp.Should().Be(user1.InternalProp);
    //     user.GetPrivate().Should().Be(user1.GetPrivate());
    // }

    #endregion Methods
}