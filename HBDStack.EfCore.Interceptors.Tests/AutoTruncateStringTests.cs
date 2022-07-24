using System.Threading.Tasks;
using FluentAssertions;
using HBDStack.EfCore.Interceptors.Tests.Infra;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HBDStack.EfCore.Interceptors.Tests;

public class AutoTruncateStringTests
{
    [Fact]
    public async Task TruncateString()
    {
        await using var context = new TestContext(new DbContextOptionsBuilder<TestContext>()
            //.UseSqlServer(ConnectionString)
            .UseInMemoryDatabase(nameof(TestContext))
            .UseAutoTruncateStringInterceptor()
            .Options);

        var item = new TestTruncateEntity
        {
            Name = "The contractor works near Yishun in Singapore but travels to Johor Bahru fortnightly to see his family. ",
            Title = "The contractor works near Yishun in Singapore but travels to Johor Bahru fortnightly to see his family. ",
        };
        
        await context.AddAsync(item);
        await context.SaveChangesAsync();

        item.Name.Should().HaveLength(10);
        item.Title.Should().HaveLength(10);
    }
    
    [Fact]
    public async Task TruncateString_Null()
    {
        await using var context = new TestContext(new DbContextOptionsBuilder<TestContext>()
            //.UseSqlServer(ConnectionString)
            .UseInMemoryDatabase(nameof(TestContext))
            .UseAutoTruncateStringInterceptor()
            .Options);

        var item = new TestTruncateEntity
        {
            Name = "The contractor works near Yishun in Singapore but travels to Johor Bahru fortnightly to see his family. ",
            Title = null,
        };
        
        await context.AddAsync(item);
        await context.SaveChangesAsync();

        item.Name.Should().HaveLength(10);
        item.Title.Should().BeNull();
    }
}