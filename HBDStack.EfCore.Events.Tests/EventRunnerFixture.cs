using System;
using DDD4Tests;
using DDD4Tests.Domains;
using Microsoft.Extensions.DependencyInjection;

namespace HBDStack.EfCore.Events.Tests;

public class EventRunnerFixture : IDisposable
{
    public EventRunnerFixture()
    {
        Provider = new ServiceCollection()
            .AddLogging()
            .AddCoreInfraServices<DddContext>(builder => builder.UseSqliteMemory(),
                enableAutoMapper:true,
                enableAutoScanEventHandler: true)
            .BuildServiceProvider();

        Context = Provider.GetRequiredService<DddContext>();
        Context.Database.EnsureCreated();

        //Add Root
        Context.Add(new Root("Steven"));
        Context.SaveChangesAsync().GetAwaiter().GetResult();
    }

    public ServiceProvider Provider { get; }
    public DddContext Context { get; }

    public void Dispose()
    {
        Provider?.Dispose();
        Context?.Dispose();
    }
}