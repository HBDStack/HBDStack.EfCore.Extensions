using System.Linq;
using DDD4Tests.Domains;
using DDD4Tests.Events;
using FluentAssertions;
using HBDStack.EfCore.Events.Internals;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace HBDStack.EfCore.Events.Tests;

public class EventFinderHandlerTests : IClassFixture<EventFinderHandlerFixture>
{
    private readonly EventFinderHandlerFixture _provider;

    public EventFinderHandlerTests(EventFinderHandlerFixture provider) => _provider = provider;

    [Fact]
    public void Scan_BeforeTypes_FromAssemblies()
    {
        var finder = _provider.Provider.GetRequiredService<EventHandlerFinder>();
        var handlers = finder.Find(HandlerTypes.BeforeAsync,
            new EntityEventItem(new Entity(), new EntityAddedEvent()));

        handlers.ToList().Count.Should().BeGreaterOrEqualTo(1);
    }
        
    [Fact]
    public void Scan_AfterTypes_FromAssemblies()
    {
        var finder = _provider.Provider.GetRequiredService<EventHandlerFinder>();
        var handlers = finder.Find(HandlerTypes.AfterAsync,
            new EntityEventItem(new Entity(), new EntityAddedEvent()));

        handlers.ToList().Count.Should().BeGreaterOrEqualTo(1);
    }
}