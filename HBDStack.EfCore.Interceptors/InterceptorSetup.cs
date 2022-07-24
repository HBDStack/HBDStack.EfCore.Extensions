// ReSharper disable CheckNamespace

using HBDStack.EfCore.Interceptors;

namespace Microsoft.EntityFrameworkCore;

public static class InterceptorSetup
{
    public static DbContextOptionsBuilder UseAutoTruncateStringInterceptor(this DbContextOptionsBuilder builder)
    {
        builder.AddInterceptors(new AutoTruncateStringInterceptor());
        return builder;
    }
    
    public static DbContextOptionsBuilder<TDbContext> UseAutoTruncateStringInterceptor<TDbContext>(this DbContextOptionsBuilder<TDbContext> builder)where TDbContext:DbContext
    {
        ((DbContextOptionsBuilder)builder).UseAutoTruncateStringInterceptor();
        return builder;
    }
}