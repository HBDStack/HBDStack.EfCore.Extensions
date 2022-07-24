using System;
using System.Runtime.InteropServices;
using HBDStack.EfCore.AuditTrail.Tests.Data;
using HBDStack.EfCore.Hooks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HBDStack.EfCore.AuditTrail.Tests;

public sealed class AuditTrailFixture:IDisposable
{
    public static string ConnectionString =>
        RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=AuditTrailDb;Integrated Security=True;Connect Timeout=30;"
            : "Data Source=localhost;Initial Catalog=AuditTrailDb;User Id=sa;Password=Pass@word1;";
    
    public ServiceProvider ServiceProvider { get;private set; }
    
    public AuditTrailFixture()
    {
        ServiceProvider = new ServiceCollection()
            .AddLogging()
            .AddDbContextWithHook<TestDbContext>(op=>op
                .UseAutoConfigModel()
                .UseInMemoryDatabase("AuditTrailDb")
                .EnableDetailedErrors())
            .AddAuditTrailServices()
            .BuildServiceProvider();
    }
    
    public void Dispose() => ServiceProvider?.Dispose();
}