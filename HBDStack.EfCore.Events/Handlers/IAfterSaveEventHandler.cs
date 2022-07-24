using HBDStack.EfCore.Abstractions.Events;

namespace HBDStack.EfCore.Events.Handlers;

/// <summary>
///     The synchronous event handler for TEvent. This only called when SaveChanges of DbContext is called
/// </summary>
/// <typeparam name="TEvent">An implementation of <see cref="IDomainEvent" /></typeparam>
// public interface IAfterSaveEventHandler<in TEvent> : IEventHandler<TEvent> where TEvent : IEventItem
// {
// }

/// <summary>
///     The asynchronous event handler for TEvent. This only called when SaveChanges of DbContext is called
/// </summary>
/// <typeparam name="TEvent">An implementation of <see cref="IDomainEvent" /></typeparam>
public interface IAfterSaveEventHandlerAsync<in TEvent> : IEventHandlerAsync<TEvent> where TEvent : IEventItem
{
}