using System.Diagnostics;
using System.Reflection;
using HBDStack.EfCore.Events;
using HBDStack.EfCore.Events.Handlers;
using HBDStack.EfCore.Events.Internals;
using HBDStack.EfCore.Events.MiddleWare;
using HBDStack.EfCore.Hooks;
using HBDStack.Framework.Extensions;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class SetupEvents
{
    /// <summary>
    ///   Add Handlers from Assemblies with ServiceLifetime
    /// </summary>
    /// <param name="services"></param>
    /// <param name="fromAssemblies">Scan the handler from assemblies</param>
    /// <returns></returns>
    public static IServiceCollection AddEvents(this IServiceCollection services, Assembly[] fromAssemblies)
    {
        var types = fromAssemblies.Extract().Class().NotAbstract().NotGeneric()
            .IsInstanceOfAny(
                //EventsExtensions.BeforeType,
                EventsExtensions.BeforeAsyncType,
                //EventsExtensions.AfterType,
                EventsExtensions.AfterAsyncType);

        return services.AsImplementedInterfaces(types)
            .AddEventRunner();
    }

    /// <summary>
    /// Add <see cref="EventRunnerHook"/> and <see cref="EventHandlerFinder"/>
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    internal static IServiceCollection AddEventRunner(this IServiceCollection services)
    {
        if (services.Any(s => s.ImplementationType == typeof(EventRunnerHook)))
        {
            Trace.WriteLine($"The {nameof(EventRunnerHook)} already registered.");
            return services;
        }

        return services
            .AddHook<EventRunnerHook>()
            .Add<EventHandlerFinder, EventHandlerFinder>();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <typeparam name="TImplementation"></typeparam>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static IServiceCollection AddEventMapper<TImplementation>(this IServiceCollection services) where TImplementation : class, IEventMapper
    {
        if (services.Any(s => s.ServiceType == typeof(IEventMapper)))
            throw new InvalidOperationException($"The {nameof(IEventMapper)} already added.");

        return services.AddScoped<IEventMapper, TImplementation>();
    }
    
    /// <summary>
    ///     Add Event Publisher
    /// </summary>
    /// <typeparam name="TImplementation">The implementation of <see cref="IEventPublisher" /></typeparam>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddEventPublisher<TImplementation>(this IServiceCollection services)
        where TImplementation : class, IEventPublisher
    {
        if (services.Any(s => s.ServiceType == typeof(IEventPublisher)))
            throw new InvalidOperationException($"The {nameof(IEventPublisher)} already added.");

        return services
            //.AddScoped(typeof(IAfterSaveEventHandler<>), typeof(InternalEventPublisherHandler<>))
            .AddScoped(typeof(IAfterSaveEventHandlerAsync<>), typeof(InternalEventPublisherHandler<>))
            //.AddScoped<TPublisherImplementation>()
            .AddScoped<IEventPublisher, TImplementation>()
            .AddEventRunner();
    }
}