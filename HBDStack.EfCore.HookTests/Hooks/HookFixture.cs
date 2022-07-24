using System;
using HBDStack.EfCore.Hooks;
using HBDStack.EfCore.HookTests.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HBDStack.EfCore.HookTests.Hooks;

public class HookFixture : IDisposable
{
    public HookFixture()
    {
        Provider = new ServiceCollection()
            .AddDbContextWithHook<HookContext>(o => o.UseSqliteMemory().UseAutoConfigModel())
            .AddHook<Hook>()
            .BuildServiceProvider();

        //Ensure Db Created
        var db = Provider.GetRequiredService<HookContext>();
        db.Database.EnsureCreated();
    }

    public ServiceProvider Provider { get; }

    public void Dispose() => Provider?.Dispose();
}