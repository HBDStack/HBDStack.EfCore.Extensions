using FluentAssertions;
using HBDStack.EfCore.Relational.Helpers.Tests.Data;
using Microsoft.EntityFrameworkCore;

namespace HBDStack.EfCore.Relational.Helpers.Tests;

public class DbContextHelperTests
{
    [Fact]
    public async Task GetTableName()
    {
        await using var db = new TestDbContext(new DbContextOptionsBuilder<TestDbContext>().UseSqlite($"Data Source={nameof(TestDbContext)}.db;").Options);
        var (schema, tableName) = db.GetTableName<TestEntity>();
        tableName.Should().Be(nameof(TestEntity));

        await db.Database.EnsureDeletedAsync();
    }

    [Fact]
    public async Task GetTableName_NotMapped()
    {
        await using var db = new TestDbContext(new DbContextOptionsBuilder<TestDbContext>().UseSqlite($"Data Source={nameof(TestDbContext)}.db;").Options);
        var (schema, tableName) = db.GetTableName<NotMappedTestEntity>();
        tableName.Should().BeNullOrEmpty();

        await db.Database.EnsureDeletedAsync();
    }

    [Fact]
    public async Task CheckTableExists_Failed()
    {
        await using var db = new TestDbContext(new DbContextOptionsBuilder<TestDbContext>().UseSqlite($"Data Source={nameof(TestDbContext)}.db;").Options);
        //await db.Database.EnsureCreatedAsync();
        var check = await db.TableExistsAsync<TestEntity>();
        check.Should().BeFalse();

        await db.Database.EnsureDeletedAsync();
    }

    [Fact]
    public async Task CheckTableExists()
    {
        await using var db = new TestDbContext(new DbContextOptionsBuilder<TestDbContext>().UseSqlite($"Data Source={nameof(TestDbContext)}.db;").Options);
        await db.Database.EnsureCreatedAsync();
        var check = await db.TableExistsAsync<TestEntity>();
        check.Should().BeTrue();

        await db.Database.EnsureDeletedAsync();
    }

    [Fact]
    public async Task CreateTable()
    {
        await using var db = new TestDbContext(new DbContextOptionsBuilder<TestDbContext>().UseSqlite($"Data Source={nameof(TestDbContext)}.db;").Options);
        await db.CreateTableAsync<TestEntity>();

        var check = await db.TableExistsAsync<TestEntity>();
        check.Should().BeTrue();

        await db.Database.EnsureDeletedAsync();
    }
}