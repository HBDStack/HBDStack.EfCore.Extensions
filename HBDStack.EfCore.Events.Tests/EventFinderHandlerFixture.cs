using System;
using DDD4Tests;
using Microsoft.Extensions.DependencyInjection;

namespace HBDStack.EfCore.Events.Tests;

public class EventFinderHandlerFixture : IDisposable
{
    public EventFinderHandlerFixture()
    {
        Provider = new ServiceCollection()
            .AddEventRunner()
            .ScanEventHandlers(new[] {typeof(DddContext).Assembly})
            .AddLogging()
            .BuildServiceProvider();
    }

    public ServiceProvider Provider { get; }

    public void Dispose()
    {
        Provider?.Dispose();
    }
}