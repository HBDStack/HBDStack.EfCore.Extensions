using System.Collections.Generic;
using System.Linq;
using DataLayer;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HBDStack.EfCore.Extensions.Tests;

[TestClass]
public class EntityEqualsTests
{
    [TestMethod]
    public void TestGetHashCode()
    {
        var user1 = new User(0, "Hello");
        var user2 = new User(-10, "BBB");

        //user1.GetHashCode().Should().NotBe(user2.GetHashCode());

        var user3 = new User(1, "Hello");
        var user4 = new User(1, "Hello");

        //user3.GetHashCode().Should().Be(user4.GetHashCode());
    }

    [TestMethod]
    public void TestReferenceEquals()
    {
        var user1 = new User(1, "Hello");
        var user2 = new User(0, "BBB");

        user1.Equals(user2).Should().BeFalse();
        user1.Equals(user1).Should().BeTrue();
        user2.Equals(user2).Should().BeTrue();
    }

    //[TestMethod]
    //public void TestEquals()
    //{
    //    var user1 = new User(1, "Hello");
    //    var user2 = new User(1, "Hello");
    //    var user3 = new User(2,"Hoang");
    //    var user4 = new User(0,"AAA");
    //    var user5 = new User(0,"BBB");

    //    user1.Equals(user2).Should().BeTrue();
    //    user1.Equals(user3).Should().BeFalse();
    //    user3.Equals(null).Should().BeFalse();
    //    user3.Equals(new Address()).Should().BeFalse();

    //    user5.Equals(user4).Should().BeFalse("Two default Key entity should not be the same");
    //}

    //[TestMethod]
    //public void TestEqualsOperator()
    //{
    //    var user1 = new User(1, "Hello");
    //    var user2 = new User(1, "Hello");
    //    var user3 = new User(2,"Hoang");

    //    (user1 == user2).Should().BeTrue();
    //    (user1 == user3).Should().BeFalse();
    //    (user3 == null).Should().BeFalse();
    //    (user3 == new Address()).Should().BeFalse();
    //}

    [TestMethod]
    public void Can_RemoveWhere_ItemFromHashSet()
    {
        var set = new HashSet<User>
        {
            new User(1, "Hoang"),
            new User(2, "Duy")
        };

        set.RemoveWhere(u => u.Id == 1);

        set.Should().HaveCount(1);
    }

    [TestMethod]
    public void Can_Remove_ItemFromHashSet()
    {
        var set = new HashSet<User>
        {
            new User(1, "Hoang"),
            new User(2, "Duy")
        };

        set.Remove(set.First());

        set.Should().HaveCount(1);
    }
}