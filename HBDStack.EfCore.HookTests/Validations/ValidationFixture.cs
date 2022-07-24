using System;
using HBDStack.EfCore.Hooks;
using HBDStack.EfCore.HookTests.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HBDStack.EfCore.HookTests.Validations;

public class ValidationFixture:IDisposable
{
    public ValidationFixture()
    {
        Provider = new ServiceCollection()
            .AddDbContextWithHook<HookContext>(o => o.UseSqliteMemory().UseAutoConfigModel())
            .AddDefaultEntityValidator()
            .BuildServiceProvider();

        //Ensure Db Created
        var db = Provider.GetRequiredService<HookContext>();
        db.Database.EnsureCreated();
    }

    public ServiceProvider Provider { get; }
    
    public void Dispose() => Provider?.Dispose();
}