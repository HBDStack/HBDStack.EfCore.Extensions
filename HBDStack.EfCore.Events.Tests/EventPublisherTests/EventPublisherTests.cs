using System.Linq;
using System.Threading.Tasks;
using DDD4Tests;
using DDD4Tests.Domains;
using DDD4Tests.Events;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace HBDStack.EfCore.Events.Tests.EventPublisherTests;

public class TestEventPublisherTests : IClassFixture<EvenPublisherFixture>
{
    private readonly EvenPublisherFixture _provider;

    public TestEventPublisherTests(EvenPublisherFixture provider) => _provider = provider;

    [Fact]
    public async Task AfterSaveTestAsync()
    {
        TestEventPublisher.Events.Clear();

        var db = _provider.Provider.GetRequiredService<DddContext>();

        var p = new Root("P1");
        p.AddEntity("A1");

        db.Add(p);
        await db.SaveChangesAsync().ConfigureAwait(false);

        TestEventPublisher.Events.Any(e=> e is EntityAddedEvent).Should().BeTrue();
    }

    [Fact]
    public async Task AfterSave_EventType_TestAsync()
    {
        TestEventPublisher.Events.Clear();

        var db = _provider.Provider.GetRequiredService<DddContext>();

        var p = new Root("P1");
        p.AddEvent<TypeEvent>();

        db.Add(p);
        await db.SaveChangesAsync().ConfigureAwait(false);

        TestEventPublisher.Events.Any(e=> e is TypeEvent).Should().BeTrue();
    }
    
    [Fact]
    public async Task BeforeSaveTestAsync()
    {
        TestEventPublisher.Events.Clear();

        var db = _provider.Provider.GetRequiredService<DddContext>();

        var p = new Root("P1");
        p.AddEntity("A1");

        db.Add(p);
        await db.SaveChangesAsync().ConfigureAwait(false);

        TestEventPublisher.Events.Any(e=> e is EntityAddedEvent).Should().BeTrue();
    }
}