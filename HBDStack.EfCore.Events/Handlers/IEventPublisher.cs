using HBDStack.EfCore.Abstractions.Events;
using Microsoft.Extensions.DependencyInjection;

namespace HBDStack.EfCore.Events.Handlers;

/// <summary>
///     Centralised event publisher.
///     All events will be route to this publisher. Use <see cref="SetupEvents.AddEventPublisher{TPublisherImplementation}"/>
/// </summary>
public interface IEventPublisher
{
    Task PublishAsync(IEventItem @event);

    public Task PublishAsync(params IEventItem[] @events) => Task.WhenAll(events.Select(PublishAsync));
}