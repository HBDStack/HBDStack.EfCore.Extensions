using System;
using System.Threading.Tasks;
using DDD4Tests.Domains;
using DDD4Tests.Events;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HBDStack.EfCore.Events.Tests;

public class AfterEventRunnerTest : IClassFixture<AfterEventRunnerFixture>
{
    private readonly AfterEventRunnerFixture _provider;

    public AfterEventRunnerTest(AfterEventRunnerFixture provider) => _provider = provider;

    [Fact]
    public async Task AddEntity_Event_ShouldRaised()
    {
        AfterEventTestHandler.ReturnFailureResult = false;
        AfterEventTestHandler.AsyncCalled = 0;
            
        var root =await _provider.Context.Set<Root>().FirstAsync().ConfigureAwait(false);

        root.AddEntity("Duy");
        await _provider.Context.SaveChangesAsync().ConfigureAwait(false);
            
        AfterEventTestHandler.AsyncCalled.Should().Be(1);
    }

    [Fact]
    public async Task Save_WithFailedResult_ExceptionShouldNOTRaised()
    {
        var root = await _provider.Context.Set<Root>().FirstAsync().ConfigureAwait(false);

        root.AddEntity("Steven");
            
        AfterEventTestHandler.ReturnFailureResult = true;

        Action a = () => _provider.Context.SaveChangesAsync().ConfigureAwait(false);
        a.Should().NotThrow();

        AfterEventTestHandler.ReturnFailureResult = false;
    }
}