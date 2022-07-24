using System.Linq;
using System.Threading.Tasks;
using DDD4Tests.Domains;
using DDD4Tests.Events;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HBDStack.EfCore.Events.Tests;

public class BeforeEventRunnerTest : IClassFixture<EventRunnerFixture>
{
    private readonly EventRunnerFixture _provider;

    public BeforeEventRunnerTest(EventRunnerFixture provider) => _provider = provider;

    [Fact]
    public async Task AddEntity_BeforeEvent_ShouldRaised()
    {
        BeforeEventTestHandler.ReturnFailureResult = false;
        BeforeEventTestHandler.AsyncCalled = false;
        //BeforeEventTestHandler.Called = false;
            
        var root = _provider.Context.Set<Root>().FirstOrDefault();

        root.AddEntity("Steven");
       await _provider.Context.SaveChangesAsync().ConfigureAwait(false);
            
        //BeforeEventTestHandler.Called.Should().BeTrue();
        BeforeEventTestHandler.AsyncCalled.Should().BeTrue();
    }

    [Fact]
    public async Task AddEntity_BeforeEventAsync_ShouldRaised()
    {
        BeforeEventTestHandler.ReturnFailureResult = false;

        var root = await _provider.Context.Set<Root>().FirstAsync().ConfigureAwait(false);

        root.AddEntity("Steven");

        await _provider.Context.SaveChangesAsync().ConfigureAwait(false);

        BeforeEventTestHandler.Called.Should().BeFalse();
        BeforeEventTestHandler.AsyncCalled.Should().BeTrue();
    }

    [Fact]
    public async Task BeforeSave_WithFailedResult_ExceptionShouldRaised()
    {
        var root = _provider.Context.Set<Root>().First();

        root.AddEntity("Steven");

        BeforeEventTestHandler.ReturnFailureResult = true;
            
        var a = () => _provider.Context.SaveChangesAsync();
       await a.Should().ThrowAsync<EventException>().ConfigureAwait(false);

        BeforeEventTestHandler.ReturnFailureResult = false;
    }
}