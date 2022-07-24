using HBDStack.EfCore.Hooks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HBDStack.EfCore.AuditTrail;

public static class EfCoreAuditTrailSetup
{
    public static IServiceCollection AddAuditTrailServices(this IServiceCollection services)
        => services
            .AddGlobalModelBuilderRegister<AuditTrailModelBuilderRegister>()
            .AddHook<AuditTrailHook>();
}