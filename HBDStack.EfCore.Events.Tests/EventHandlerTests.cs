using System.Linq;
using DDD4Tests;
using DDD4Tests.Events;
using FluentAssertions;
using HBDStack.EfCore.Events.Handlers;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace HBDStack.EfCore.Events.Tests;

public class EventHandlerTests
{
    [Fact]
    public void Scan_Types_FromAssemblies()
    {
        var provider = new ServiceCollection()
            .ScanEventHandlers(new[]{typeof(DddContext).Assembly});

        provider.Count.Should().BeGreaterOrEqualTo(1);
        provider.Any(p => p.ServiceType == typeof(IBeforeSaveEventHandlerAsync<EntityAddedEvent>)).Should().BeTrue();
    }
}