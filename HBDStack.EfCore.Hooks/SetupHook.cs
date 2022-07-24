using System.Diagnostics;
using HBDStack.EfCore.Hooks.Internals;
using HBDStack.Framework.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HBDStack.EfCore.Hooks;

public static class SetupEfCoreHook
{
    /// <summary>
    /// Add HookRunner from ServiceProvider to DbContext Interceptors.
    /// </summary>
    /// <param name="options"></param>
    /// <param name="provider"></param>
    /// <returns></returns>
    public static DbContextOptionsBuilder AddHookInterceptor(this DbContextOptionsBuilder options,
        IServiceProvider provider) =>
        options.AddInterceptors(provider.GetRequiredService<HookRunner>());
}

// ReSharper disable once CheckNamespace
public static class SetupHook
{
    /// <summary>
    ///     Add Implementation of <see cref="IHook" /> or <see cref="IHookAsync" /> to <see cref="IServiceCollection" />
    /// </summary>
    /// <param name="services"></param>
    /// <param name="lifetime"></param>
    /// <returns></returns>
    public static IServiceCollection AddHook<THook>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped) where THook : class
    {
        var type = typeof(THook);

        if (services.Any(s => s.ServiceType == type))
        {
            Trace.WriteLine($"The Hook {type.Name} already added.");
            return services;
        }

        var added = false;

        // if (type.IsImplementOf(typeof(IHook)))
        // {
        //     services.AddScoped(p => p.GetRequiredService<THook>() as IBeforeSaveHook);
        //     services.AddScoped(p => p.GetRequiredService<THook>() as IAfterSaveHook);
        //     added = true;
        // }

        if (type.IsImplementOf(typeof(IHookAsync)))
        {
            services.AddScoped(p => (IBeforeSaveHookAsync)p.GetRequiredService<THook>() );
            services.AddScoped(p => (IAfterSaveHookAsync)p.GetRequiredService<THook>());
            added = true;
        }

        return added
            ? services.Add<THook, THook>(lifetime)
            : services.AsImplementedInterfaces(new[] { type }, lifetime);
    }

    /// <summary>
    ///     Al registered IHooks need this runner to run. If not provided they will be ignored.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="lifetime"></param>
    /// <returns></returns>
    public static IServiceCollection AddHookRunner(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped)
    {
        if (services.Any(s => s.ServiceType == typeof(HookRunner)))
        {
            Trace.WriteLine("The HookRunner already registered.");
            return services;
        }

        return services.Add<HookRunner, HookRunner>(lifetime);
    }

    /// <summary>
    ///     Add Hook Runner to <see cref="IServiceCollection" /> and register the Hook Interceptor to
    /// </summary>
    ///     <see cref="DbContextOptionsBuilder" />
    ///     <param name="services"></param>
    ///     <param name="builder"></param>
    ///     <param name="contextLifetime"></param>
    ///     <param name="optionLifetime"></param>
    ///     <returns></returns>
    /// 
    public static IServiceCollection AddDbContextWithHook<TDbContext>(this IServiceCollection services,
        Action<DbContextOptionsBuilder> builder,
        ServiceLifetime contextLifetime = ServiceLifetime.Scoped,
        ServiceLifetime optionLifetime = ServiceLifetime.Scoped) where TDbContext : DbContext
    {
        services
            .AddHookRunner(contextLifetime)
            .AddDbContext<TDbContext>((provider, options) =>
            {
                builder(options);
                options.AddHookInterceptor(provider);
            }, contextLifetime, optionLifetime);

        return services;
    }
}