using System.Linq;
using System.Threading.Tasks;
using DDD4Tests;
using DDD4Tests.Domains;
using DDD4Tests.Events;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace HBDStack.EfCore.Events.Tests.EventPublisherTests;

public class AutoEventWithAutoMapperTests : IClassFixture<EvenPublisherFixture>
{
    private readonly EvenPublisherFixture _provider;

    public AutoEventWithAutoMapperTests(EvenPublisherFixture provider) => _provider = provider;

    [Fact]
    public async Task CreatedTest()
    {
        TestEventPublisher.Events.Clear();

        var db = _provider.Provider.GetRequiredService<DddContext>();

        var p = new Root("P1");

        db.Add(p);
       await db.SaveChangesAsync().ConfigureAwait(false);

        TestEventPublisher.Events.Any(e=>e is EntityAddedEvent).Should().BeTrue();
    }

    [Fact]
    public async Task CreatedAsyncTest()
    {
        TestEventPublisher.Events.Clear();

        var db = _provider.Provider.GetRequiredService<DddContext>();

        var p = new Root("P1");

        db.Add(p);
        await db.SaveChangesAsync().ConfigureAwait(false);

        TestEventPublisher.Events.Any(e=>e is EntityAddedEvent).Should().BeTrue();
    }

    [Fact]
    public async Task UpdatedTest()
    {
        var db = _provider.Provider.GetRequiredService<DddContext>();
        var p = new Root("P1");
        //Added 
        db.Add(p);
      await  db.SaveChangesAsync().ConfigureAwait(false);
      
        TestEventPublisher.Events.Clear();
        //Update
        p.SetUpdatedBy("Unit test");
      await  db.SaveChangesAsync().ConfigureAwait(false);

      TestEventPublisher.Events.Any(e=>e is EntityUpdatedEvent).Should().BeTrue();
    }

    [Fact]
    public async Task UpdatedAsyncTest()
    {
        var db = _provider.Provider.GetRequiredService<DddContext>();
        var p = new Root("P1");
        //Added 
        db.Add(p);
        await db.SaveChangesAsync().ConfigureAwait(false);
        
        TestEventPublisher.Events.Clear();

        //Update
        p.SetUpdatedBy("Unit test");
        await db.SaveChangesAsync().ConfigureAwait(false);

        TestEventPublisher.Events.Any(e=>e is EntityUpdatedEvent).Should().BeTrue();
    }

    [Fact]
    public async Task DeletedAsyncTest()
    {
        var db = _provider.Provider.GetRequiredService<DddContext>();
        var p = new Root("P1");

        //Added 
        db.Add(p);
        await db.SaveChangesAsync().ConfigureAwait(false);
        TestEventPublisher.Events.Clear();

        //Delete
        db.Set<Root>().Remove(p);
        await db.SaveChangesAsync().ConfigureAwait(false);

        TestEventPublisher.Events.Any(e=>e is EntityDeletedEvent).Should().BeTrue();
    }
}