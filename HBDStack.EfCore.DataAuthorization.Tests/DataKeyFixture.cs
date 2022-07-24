using System;
using DDD4Tests;
using DDD4Tests.Domains;
using HBDStack.EfCore.Hooks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HBDStack.EfCore.DataAuthorization.Tests;

public class DataKeyFixture : IDisposable
{
    public DataKeyFixture()
    {
        Provider = new ServiceCollection()
            .AddLogging()
            .AddDataKeyProvider<TestDataKeyProvider>(ServiceLifetime.Singleton)
            .AddDbContextWithHook<DddContext>(builder =>
                builder.UseSqliteMemory()
                    .UseAutoConfigModel())
            .BuildServiceProvider();

        Context = Provider.GetRequiredService<DddContext>();
        Context.Database.EnsureCreated();

        Context.Set<Root>()
            .AddRange(new Root("Duy"), new Root("Steven"), new Root("Hoang"), new Root("HBD"));

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