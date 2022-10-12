using System.Diagnostics;
using System.Reflection;
using HBDStack.EfCore.Events;
using HBDStack.EfCore.Events.Handlers;
using HBDStack.EfCore.Events.Internals;
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
    /// <param name="fromAssemblies"></param>
    /// <returns></returns>
    public static IServiceCollection ScanEventHandlers(this IServiceCollection services,
        Assembly[] fromAssemblies)
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
    ///     Add Event Publisher
    /// </summary>
    /// <typeparam name="TPublisherImplementation">The implementation of <see cref="IEventPublisher" /></typeparam>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddEventPublisher<TPublisherImplementation>(this IServiceCollection services)
        where TPublisherImplementation : class, IEventPublisher
    {
        if (services.Any(s => s.ServiceType == typeof(IEventPublisher)))
            throw new InvalidOperationException($"The {nameof(IEventPublisher)} already added.");

        return services
            //.AddScoped(typeof(IAfterSaveEventHandler<>), typeof(InternalEventPublisherHandler<>))
            .AddScoped(typeof(IAfterSaveEventHandlerAsync<>), typeof(InternalEventPublisherHandler<>))
            //.AddScoped<TPublisherImplementation>()
            .AddScoped<IEventPublisher, TPublisherImplementation>()
            .AddEventRunner();
    }
}