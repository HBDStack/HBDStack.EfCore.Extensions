using DataLayer;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HBDStack.EfCore.Extensions.Tests;

[TestClass]
public class DefaultKeyTests
{
    [TestMethod]
    public void DefaultKey()
    {
        new User().Id.Should().Be(0);
        new Address().Id.Should().Be(0);
    }
}