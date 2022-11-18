using System;
using DDD4Tests;
using DDD4Tests.Domains;
using DDD4Tests.Events;
using HBDStack.EfCore.Events.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace HBDStack.EfCore.Events.Tests;

public class AfterEventRunnerFixture : IDisposable
{
    public AfterEventRunnerFixture()
    {
        Provider = new ServiceCollection()
            .AddLogging()
               
            .AddCoreInfraServices<DddContext>(builder => builder.UseSqliteMemory(),
                enableAutoMapper:true,
                enableAutoScanEventHandler: false)
            //If you disable the scan the Hook and Even runner wont be added by default.
            .AddSingleton<IAfterSaveEventHandlerAsync<EntityAddedEvent>, AfterAddedEventTestHandler>()
            //You need to add it manually
            .AddEventRunner()
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