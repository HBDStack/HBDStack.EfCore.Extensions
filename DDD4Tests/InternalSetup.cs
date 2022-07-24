using System;
using System.Linq.Expressions;
using System.Reflection;
using AutoMapper;
using HBDStack.EfCore.Hooks;
using Microsoft.EntityFrameworkCore;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
///     The DI setup for Infra
/// </summary>
public static class InternalSetup
{
    /// <summary>
    ///     Add Boundary Context with DomainServices
    /// </summary>
    /// <typeparam name="TContext">The BondedContext from <see cref="DbContext" /> </typeparam>
    /// <param name="service"></param>
    /// <param name="contextBuilder"></param>
    /// <param name="assembliesToScans">
    ///     The assemblies to scan all Entities, Configuration The TContext assembly will be use if
    ///     not provided.
    /// </param>
    /// <param name="entityFilter"></param>
    /// <param name="contextLifetime"></param>
    /// <param name="optionsLifetime"></param>
    /// <returns></returns>
    internal static IServiceCollection AddBoundedContext<TContext>(this IServiceCollection service,
        Action<DbContextOptionsBuilder> contextBuilder,
        Assembly[] assembliesToScans = null,
        Expression<Func<Type, bool>> entityFilter = null,
        ServiceLifetime contextLifetime = ServiceLifetime.Scoped,
        ServiceLifetime optionsLifetime = ServiceLifetime.Scoped) where TContext : DbContext
    {
        assembliesToScans ??= new[] {typeof(TContext).Assembly};

        void OptionFunc(DbContextOptionsBuilder op)
        {
            contextBuilder(op);
            op.UseAutoConfigModel(o =>
            {
                var scan = o.ScanFrom(assembliesToScans);

                if (entityFilter != null) scan.WithFilter(entityFilter);//.IgnoreOthers();
            });
        }
            
        service
            .AddDbContextWithHook<TContext>(OptionFunc, contextLifetime, optionsLifetime);

        return service;
    }

    /// <summary>
    ///     The wrapper method to add all needed components for infra
    ///     - Add Scan Event Handlers from Assemblies />
    ///     - Call <see cref="AddBoundedContext{TContext}" />
    ///     - Call Automapper registration.
    /// </summary>
    /// <param name="service"></param>
    /// <param name="contextBuilder"></param>
    /// <param name="assembliesToScans"></param>
    /// <param name="entityFilter">
    ///     Customize entity scanning filtering. Default all classes that inherited IEntity will be
    ///     included. This is useful when you want to filter out the difference entities that belong different DbContext.
    /// </param>
    /// <param name="contextLifetime">The life time of DbContext. Default is `Scope`</param>
    /// <param name="optionsLifetime">The life time of DbContext Options. Default is `Scope`</param>
    /// <param name="autoMapperConfig">
    ///     Enable auto mapper config. Default is `false` it mean you should config auto mapper by
    ///     your-self.
    /// </param>
    /// <param name="eventHandlersLifetime">
    ///     The life time of Event Handler that will be auto-scan from assemblies. Default is
    ///     `Scope`
    /// </param>
    /// <param name="enableAutoScanEventHandler">
    ///     Auto scan all Event handlers from assembliesToScans. If disable this you may
    ///     need to register the event handler by your self.
    /// </param>
    /// <param name="enableAutoMapper">turn this to false to ignore automapper setup. You need to setup if manually.</param>
    /// <typeparam name="TContext"></typeparam>
    /// <returns></returns>
    public static IServiceCollection AddCoreInfraServices<TContext>(this IServiceCollection service,
        Action<DbContextOptionsBuilder> contextBuilder,
        Assembly[] assembliesToScans = null,
        Expression<Func<Type, bool>> entityFilter = null,
        ServiceLifetime contextLifetime = ServiceLifetime.Scoped,
        ServiceLifetime optionsLifetime = ServiceLifetime.Scoped,
        bool enableAutoScanEventHandler = true,
        bool enableAutoMapper = false,
        Action<IMapperConfigurationExpression> autoMapperConfig = null) where TContext : DbContext
    {
        assembliesToScans ??= new[] {typeof(TContext).Assembly};

        if (enableAutoScanEventHandler) service.ScanEventHandlers(assembliesToScans);

        if (enableAutoMapper)
        {
            //Auto Mapper
            autoMapperConfig ??= cf => cf.ShouldUseConstructor = f => f.IsPublic;
            service.AddAutoMapper(autoMapperConfig, assembliesToScans);
        }

        return service.AddBoundedContext<TContext>(contextBuilder,
            assembliesToScans,
            entityFilter,
            contextLifetime,
            optionsLifetime);
    }
}