using System;
using DDD4Tests;
using DDD4Tests.Domains;
using DDD4Tests.Events;
using HBDStack.EfCore.Hooks;
using HBDStack.ObjectMapper.AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HBDStack.EfCore.Events.Tests.EventPublisherTests;

public class EvenPublisherFixture : IDisposable
{
    public EvenPublisherFixture()
    {
        Provider = new ServiceCollection()
            .AddLogging()
            .AddEventPublisher<TestEventPublisher>()
            .AddAutoObjectMapper(profileAssemblies: typeof(DddContext).Assembly)
            .AddDbContextWithHook<DddContext>(builder => builder.UseSqliteMemory().UseAutoConfigModel())
            .BuildServiceProvider();

        Context = Provider.GetRequiredService<DddContext>();
        Context.Database.EnsureCreated();


        Context.Set<Root>()
            .AddRange(new Root("Duy"), new Root("Steven"), new Root("Hoang"), new Root("HBD"));

        BeforeAddedEventTestHandler.ReturnFailureResult = false;
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