using System;
using System.Threading.Tasks;
using DataLayer;
using FluentAssertions;
using HBDStack.EfCore.Repos.Abstractions.Basic;
using HBDStack.EfCore.Repos.Basic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HBDStack.EfCore.Extensions.Tests;

/// <summary>
/// Concurrent update need to be tested with real SQL Server.
/// </summary>
[TestClass]
public class ConcurrentUpdateWithSqlTests
{
    private static MyDbContext _db;
    private static MyDbContext _db1;

    [ClassCleanup]
    public static void CleanUp()
    {
        //_db.Database.EnsureDeleted();
        _db.Dispose();
    }

    [ClassInitialize]
    public static void Setup(TestContext _)
    {
        var options = new DbContextOptionsBuilder()
            .LogTo(Console.WriteLine, (eventId, logLevel) => logLevel >= LogLevel.Information
                                                             || eventId == RelationalEventId.CommandExecuting)
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors()
            .UseSqlServer(WithSqlDbTests.ConnectionString)
            .UseAutoConfigModel(op => op.ScanFrom(typeof(MyDbContext).Assembly))
            .Options;
        
        _db = new MyDbContext(options);
        _db1 = new MyDbContext(options);
        
        _db.Database.EnsureCreated();
    }

    [TestMethod]
    public async Task ConcurrencyWithDbContextTest()
    {
        //1. Create a new User.
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

        await _db.AddAsync(user);
        await _db.SaveChangesAsync();

        user = await _db.Set<User>().FirstAsync(u => u.Id == user.Id);
        var createdVersion = (byte[])user.RowVersion.Clone();

        //2. Update user with created version. It should allow to update.
        // Change the person's name in the database to simulate a concurrency conflict.
        await _db.Database.ExecuteSqlRawAsync(
            $"UPDATE [User] SET [FirstName] = 'Duy2' WHERE Id = {user.Id}");

        //3. Update user with created version again. It should NOT allow to update.
        user.FirstName = "Duy3";
        user.SetUpdatedBy("System");

        //The DbUpdateConcurrencyException will be throw here
        Func<Task> fun = () => _db.SaveChangesAsync();
        await fun.Should().ThrowAsync<DbUpdateConcurrencyException>();

        //4. Check the RowVersion in Db
        await _db.Entry(user).ReloadAsync();
        var updatedVersion = (byte[])user.RowVersion.Clone();

        updatedVersion.Should().NotEqual(createdVersion);

        //5. Update user again with latest row version.
        //NOTE this will overwrite the changes in Database. Check the latest data before execute.
        await _db.SaveChangesAsync();
    }

    [TestMethod]
    public async Task ConcurrencyWithRepositoryTest()
    {
        IBasicRepository repo = new BasicRepository(_db);
        IBasicRepository repo1 = new BasicRepository(_db1);
        //1. Create a new User.
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

        await repo.AddAsync(user);
        await repo.SaveAsync();

        var createdVersion = (byte[])user.RowVersion.Clone();
        
        //2. Update user with created version. It should allow to update.
        // Change the person's name in the database to simulate a concurrency conflict.
        user.FirstName = "Duy3";
        user.SetUpdatedBy("System");
        await repo.SaveAsync();
        await repo.ReloadAsync(user);
        Console.WriteLine("Latest Version: " + user.RowVersion);

        //3. Update user with created version again. It should NOT allow to update.
        user = await repo.Get<User>().AsNoTracking().FirstAsync(u => u.Id == user.Id);
        user.FirstName = "Duy3";
        repo1.UpdateRowVersion(user, createdVersion);
        
        //The DbUpdateConcurrencyException will be throw here
        Func<Task> fun= async ()=> await repo1.UpdateAndSaveAsync(user);
        await fun.Should().ThrowAsync<DbUpdateConcurrencyException>();
    }
}