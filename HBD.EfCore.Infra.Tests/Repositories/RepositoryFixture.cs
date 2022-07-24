using System;
using DDD4Tests;
using DDD4Tests.Domains;
using Microsoft.Extensions.DependencyInjection;

namespace HBD.EfCore.Infra.Tests.Repositories;

public class RepositoryFixture : IDisposable
{
    public RepositoryFixture()
    {
        Provider = new ServiceCollection()
            .AddLogging()
            .AddGenericRepositories<DddContext>()
            .AddCoreInfraServices<DddContext>(
                builder => builder.UseSqliteMemory(),
                enableAutoMapper: true)
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