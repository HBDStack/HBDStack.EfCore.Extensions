using System.Linq;
using DataLayer;
using DataLayer.Mappers;
using FluentAssertions;
using HBDStack.EfCore.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HBDStack.EfCore.Extensions.Tests;

[TestClass]
public class EntityAutoConfigTests
{
    [TestMethod]
    public void ScanEntites()
    {
        var info =
            new RegistrationInfo(typeof(MyDbContext).Assembly).WithDefaultMappersType(typeof(BaseEntityMapper<>));
        var entities = info.GetAllEntityTypes();

        entities.Should().NotBeEmpty();
        entities.Count().Should().BeGreaterOrEqualTo(11);
    }

    [TestMethod]
    public void ScanConfigs_Should_IncludeEntities_ThatNotInherit_IEntity()
    {
        var info =
            new RegistrationInfo(typeof(MyDbContext).Assembly).WithDefaultMappersType(typeof(BaseEntityMapper<>));
        var configs = info.GetDefinedMappers();

        configs.Should().NotBeEmpty();
        configs.Any(c => c == typeof(NotInheritIEntityConfig)).Should().BeTrue();
    }

    [TestMethod]
    public void ScanGenericConfigs()
    {
        var info =
            new RegistrationInfo(typeof(MyDbContext).Assembly).WithDefaultMappersType(typeof(BaseEntityMapper<>));
        var configs = info.GetGenericMappers();

        configs.Should().NotBeEmpty();
        configs.Count().Should().Be(2);
    }
}