using System;
using FluentAssertions;
using HBDStack.EfCore.Abstractions.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HBDStack.EfCore.Extensions.Tests;

[TestClass]
public class EntityTypesTests
{
    #region Public Methods

    [TestMethod]
    public void TestEntity_Audit()
    {
        typeof(Entity<int>).IsAssignableFrom(typeof(AuditEntity<int>))
            .Should().BeTrue();

        typeof(Entity<int>).IsAssignableFrom(typeof(AuditEntity<int>))
            .Should().BeTrue();

        typeof(IEntity<int>).IsAssignableFrom(typeof(AuditEntity<int>))
            .Should().BeTrue();

        typeof(IEntity<int>).IsAssignableFrom(typeof(AuditEntity<int>))
            .Should().BeTrue();
    }

    [TestMethod]
    public void TestEntity_AuditEntityGeneric()
    {
        typeof(IEntity<int>).IsAssignableFrom(typeof(IAuditEntity<int>))
            .Should().BeTrue();
    }

    [TestMethod]
    public void TestEntity_AuditGeneric()
    {
        typeof(IEntity<int>).IsAssignableFrom(typeof(IAuditEntity<int>))
            .Should().BeTrue();
    }

    [TestMethod]
    public void TestEntityGeneric_AuditGeneric()
    {
        typeof(IEntity<int>).IsAssignableFrom(typeof(IAuditEntity<int>))
            .Should().BeTrue();
    }

    [TestMethod]
    public void TestIAudit_Audit()
    {
        typeof(IAuditEntity<int>).IsAssignableFrom(typeof(AuditEntity<int>))
            .Should().BeTrue();

        typeof(IAuditEntity<Guid>).IsAssignableFrom(typeof(AuditEntityGuid))
            .Should().BeTrue();
    }

    [TestMethod]
    public void TestIEntity_Entity()
    {
        typeof(IEntity<int>).IsAssignableFrom(typeof(Entity<int>))
            .Should().BeTrue();

        typeof(IEntity<Guid>).IsAssignableFrom(typeof(EntityGuid))
            .Should().BeTrue();
    }

    [TestMethod]
    public void TestIEntity_IEntityGeneric()
    {
        typeof(IEntity<int>).IsAssignableFrom(typeof(IEntity<int>))
            .Should().BeTrue();
    }

    #endregion Public Methods
}