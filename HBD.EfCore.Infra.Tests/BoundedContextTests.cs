using DDD4Tests;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace HBD.EfCore.Infra.Tests;

public class BoundedContextTests
{
    [Fact]
    public void AbleToAddBoundedContext_Without_DataKeyProvider()
    {
        var provider = new ServiceCollection()
            .AddLogging()
            .AddCoreInfraServices<DddContext>(b => { }, assembliesToScans: new[] {typeof(DddContext).Assembly}, enableAutoMapper:true)
            .BuildServiceProvider();

        var c = provider.GetRequiredService<DddContext>();
        c.Should().NotBeNull();
        c._dataKeyProvider.Should().BeNull();
    }

    [Fact]
    public void AbleToAddBoundedContext_With_DataKeyProvider()
    {
        var provider = new ServiceCollection()
            .AddLogging()
            .AddDataKeyProvider<TestDataKeyProvider>()
            .AddCoreInfraServices<DddContext>(b => { }, assembliesToScans: new[] {typeof(DddContext).Assembly}, enableAutoMapper:true)
            .BuildServiceProvider();

        var c = provider.GetRequiredService<DddContext>();
        c._dataKeyProvider.Should().NotBeNull();
    }
}