using System;
using System.Threading.Tasks;
using DataLayer;
using FluentAssertions;
using HBDStack.EfCore.Abstractions.Entities;
using HBDStack.EfCore.Extensions.Configurations;
using HBDStack.EfCore.Extensions.Internal;
using HBDStack.TestHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HBDStack.EfCore.Extensions.Tests;

[TestClass]
public class RegisterTests
{
    #region Methods

    [TestMethod]
    public async Task Test_RegisterEntities_DefaultOptions()
    {
        //Create User with Address
        await UnitTestSetup.Db.Set<User>().AddAsync(new User("Duy")
        {
            FirstName = "Duy",
            LastName = "Hoang",
            Addresses =
            {
                new Address
                {
                    Street = "12"
                }
            }
        });

        await UnitTestSetup.Db.SaveChangesAsync().ConfigureAwait(false);

        Assert.IsTrue(await UnitTestSetup.Db.Set<User>().CountAsync().ConfigureAwait(false) >= 1);
        Assert.IsTrue(await UnitTestSetup.Db.Set<Address>().CountAsync().ConfigureAwait(false) >= 1);
    }

    [TestMethod]
    public async Task TestAccountStatusDataSeeding()
    {
        await using var db = new MyDbContext(new DbContextOptionsBuilder()
            .UseSqliteMemory()

            //No Assembly provided it will scan the MyDbContext assembly.
            .UseAutoConfigModel()
            .Options);
        await db.Database.EnsureCreatedAsync().ConfigureAwait(false);
        (await db.Set<AccountStatus>().CountAsync().ConfigureAwait(false)).Should().BeGreaterOrEqualTo(2);
    }

    [TestMethod]
    public async Task TestCreateDb()
    {
        await using var db = new MyDbContext(new DbContextOptionsBuilder()
            .UseSqliteMemory()
            .UseAutoConfigModel(op => op.ScanFrom(typeof(MyDbContext).Assembly)
                .WithDefaultMappersType(typeof(EntityTypeConfiguration<>)))
            .Options);

        await db.Database.EnsureCreatedAsync().ConfigureAwait(false);

        //Create User with Address
        await db.Set<User>().AddAsync(new User("Duy")
        {
            FirstName = "Duy",
            LastName = "Hoang",
            Addresses =
            {
                new Address
                {
                    Street = "12"
                }
            }
        });

        await db.SaveChangesAsync().ConfigureAwait(false);

        var users = await db.Set<User>().ToListAsync().ConfigureAwait(false);
        var adds = await db.Set<Address>().ToListAsync().ConfigureAwait(false);

        Assert.IsTrue(users.Count == 1);
        Assert.IsTrue(adds.Count == 1);
    }

    [TestMethod]

    //[ExpectedException(typeof(DbUpdateException))]
    public async Task TestCreateDb_CustomMapper()
    {
        await using var db = new MyDbContext(new DbContextOptionsBuilder()
            .UseSqliteMemory()

            //No Assembly provided it will scan the MyDbContext assembly.
            .UseAutoConfigModel(op => op.ScanFrom(typeof(MyDbContext).Assembly)
                .WithDefaultMappersType(typeof(EntityTypeConfiguration<>)))
            .Options);
        await db.Database.EnsureCreatedAsync().ConfigureAwait(false);

        //Create User with Address
        await db.Set<User>().AddAsync(new User("Duy")
        {
            FirstName = "Duy",
            LastName = "Hoang",
            Addresses =
            {
                new Address {Street = "123"}
            }
        });

        await db.SaveChangesAsync().ConfigureAwait(false);

        (await db.Set<Address>().AnyAsync().ConfigureAwait(false)).Should().BeTrue();
    }

    [TestMethod]
    public void TestCreateDb_NoAssembly()
    {
        using var db = new MyDbContext(new DbContextOptionsBuilder()
            .UseSqliteMemory()

            //No Assembly provided it will scan the MyDbContext assembly.
            .UseAutoConfigModel(op => op.ScanFrom()
                .WithDefaultMappersType(typeof(EntityTypeConfiguration<>)))
            .Options);
    }

    [TestMethod]
    public void TestCreateDb_WithAssembly()
    {
        using var db = new MyDbContext(new DbContextOptionsBuilder<MyDbContext>()
            //No Assembly provided it will scan the MyDbContext assembly.
            .UseAutoConfigModel(op => op.ScanFrom(typeof(MyDbContext).Assembly))
            .UseSqliteMemory()
            .Options);
    }

    [TestMethod]

    //[ExpectedException(typeof(DbUpdateException))]
    public async Task TestCreateDb_Validate()
    {
        await using var db = new MyDbContext(new DbContextOptionsBuilder()
            .UseSqliteMemory()
            .UseAutoConfigModel(op => op.ScanFrom(typeof(MyDbContext).Assembly)
                .WithDefaultMappersType(typeof(EntityTypeConfiguration<>)))
            .Options);
        await db.Database.EnsureCreatedAsync().ConfigureAwait(false);

        //Create User with Address
        await db.Set<User>().AddAsync(new User("Duy")
        {
            FirstName = "Duy",
            LastName = "Hoang",
            Addresses =
            {
                new Address {Street = "123"}
            }
        });

        await db.SaveChangesAsync().ConfigureAwait(false);
    }

    [TestMethod]
    public async Task TestEnumStatus1DataSeeding()
    {
        await using var db = new MyDbContext(new DbContextOptionsBuilder()
            .UseSqliteMemory()

            //No Assembly provided it will scan the MyDbContext assembly.
            .UseAutoConfigModel()
            .Options);
        await db.Database.EnsureCreatedAsync().ConfigureAwait(false);
        (await db.Set<EnumTables<EnumStatus1>>().CountAsync().ConfigureAwait(false)).Should().Be(3);
    }

    [TestMethod]
    public async Task TestEnumStatusDataSeeding()
    {
        await using var db = new MyDbContext(new DbContextOptionsBuilder()
            .UseSqliteMemory()

            //No Assembly provided it will scan the MyDbContext assembly.
            .UseAutoConfigModel()
            .Options);
        await db.Database.EnsureCreatedAsync().ConfigureAwait(false);
        (await db.Set<EnumTables<EnumStatus>>().CountAsync().ConfigureAwait(false)).Should().Be(3);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public async Task TestIgnoredEntityAsync()
    {
        await using var db = new MyDbContext(new DbContextOptionsBuilder()
            .UseSqliteMemory()
            .UseAutoConfigModel(op =>
                op.ScanFrom(typeof(MyDbContext).Assembly)
                    .WithDefaultMappersType(typeof(EntityTypeConfiguration<>)))
            .Options);

        db.Database.EnsureCreated();

        var list = await db.Set<IgnoredAutoMapperEntity>().ToListAsync();
    }

    [TestMethod]

    //[Ignore]
    [ExpectedException(typeof(ArgumentException))]
    public void TestWithCustomEntityMapper_Bad()
    {
        using var db = new MyDbContext(new DbContextOptionsBuilder()
            .UseSqliteMemory()
            .UseAutoConfigModel(op =>
                op.ScanFrom(typeof(MyDbContext).Assembly).WithDefaultMappersType(typeof(Entity<>)))
            .Options);

        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void TestWithCustomEntityMapper_NullFilter_Bad()
    {
        using var db = new MyDbContext(new DbContextOptionsBuilder()
            .UseSqliteMemory()
            .UseAutoConfigModel(op =>
                op.ScanFrom(typeof(MyDbContext).Assembly).WithFilter(null))
            .Options);
    }

    #endregion Methods
}