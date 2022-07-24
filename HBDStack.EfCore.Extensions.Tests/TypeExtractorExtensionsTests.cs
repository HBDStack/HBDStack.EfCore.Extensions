using System.Linq;
using BenchmarkDotNet.Attributes;
using DataLayer;
using FluentAssertions;
using HBDStack.EfCore.Abstractions.Attributes;
using HBDStack.EfCore.Abstractions.Entities;
using HBDStack.Framework.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HBDStack.EfCore.Extensions.Tests;

[TestClass]
public class TypeExtractorExtensionsTests
{
    #region Methods

    [TestMethod]
    [Benchmark]
    public void Test_Abstract()
    {
        typeof(MyDbContext).Assembly.Extract().Abstract()
            .Should().HaveCountGreaterOrEqualTo(1);
    }

    [TestMethod]
    [Benchmark]
    public void Test_HasAttribute()
    {
        typeof(MyDbContext).Assembly.Extract().HasAttribute<StaticDataAttribute>()
            .Count().Should().BeGreaterOrEqualTo(1);
    }

    [TestMethod]
    [Benchmark]
    public void Test_Interface()
    {
        typeof(MyDbContext).Assembly.Extract().IsInstanceOf<BaseEntity>()
            .Count().Should().BeGreaterOrEqualTo(1);
    }

    [TestMethod]
    [Benchmark]
    public void Test_Nested()
    {
        typeof(MyDbContext).Assembly.Extract().Nested()
            .Should().HaveCountGreaterOrEqualTo(1);
    }

    [TestMethod]
    [Benchmark]
    public void Test_NotClass()
    {
        typeof(MyDbContext).Assembly.Extract().NotClass()
            .Should().HaveCountGreaterOrEqualTo(1);
    }

    [TestMethod]
    [Benchmark]
    public void Test_NotEnum()
    {
        typeof(MyDbContext).Assembly.Extract().NotEnum()
            .Should().HaveCountGreaterOrEqualTo(1);
    }

    [TestMethod]
    [Benchmark]
    public void TestExtract()
    {
        typeof(MyDbContext).Assembly.Extract().Public().Class().Count()
            .Should().BeGreaterOrEqualTo(3);
    }

    [TestMethod]
    [Benchmark]
    public void TestExtract_GenericClass()
    {
        var list = typeof(MyDbContext).Assembly.ScanGenericClassesWithFilter("Mapper").ToList();
        list.Any().Should().BeTrue();
    }

    [TestMethod]
    [Benchmark]
    public void TestExtract_NotInstanceOf()
    {
        var list = typeof(MyDbContext).Assembly.Extract().Class().NotInstanceOf(typeof(IEntity<>)).ToList();
        list.Contains(typeof(User)).Should().BeFalse();
        list.Contains(typeof(Address)).Should().BeFalse();
    }

    [TestMethod]
    [Benchmark]
    public void TestScanClassesFromWithFilter()
    {
        typeof(MyDbContext).Assembly.ScanClassesWithFilter("Mapper")
            .Count().Should().BeGreaterOrEqualTo(1);
    }

    [TestMethod]
    [Benchmark]
    public void TestScanClassesImplementOf()
    {
        typeof(MyDbContext).Assembly.ScanClassesImplementOf(typeof(Entity<int>))
            .Count().Should().BeGreaterOrEqualTo(1);
    }

    [TestMethod]
    [Benchmark]
    public void TestScanClassesImplementOf_Generic()
    {
        typeof(MyDbContext).Assembly.ScanClassesImplementOf<IEntity<int>>()
            .Count().Should().BeGreaterOrEqualTo(2);
    }

    [TestMethod]
    [Benchmark]
    public void TestScanPublicClassesFromWithFilter()
    {
        typeof(MyDbContext).Assembly.ScanPublicClassesWithFilter("Context")
            .Count().Should().Be(1);
    }

    [TestMethod]
    [Benchmark]
    public void TestScanPublicClassesImplementOf()
    {
        typeof(MyDbContext).Assembly.ScanPublicClassesImplementOf<AuditEntity<int>>()
            .Count().Should().BeGreaterOrEqualTo(1);
    }

    #endregion Methods
}