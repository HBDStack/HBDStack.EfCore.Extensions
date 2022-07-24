using HBDStack.EfCore.Abstractions.Events;

namespace HBDStack.EfCore.Events.Handlers;

/// <summary>
///     The synchronous event handler for TEvent. This only called when calling SaveChanges of DbContext
/// </summary>
/// <typeparam name="TEvent">An implementation of <see cref="IEventItem" /></typeparam>
// public interface IBeforeSaveEventHandler<in TEvent> : IEventHandler<TEvent> where TEvent : IEventItem
// {
// }

/// <summary>
///     The asynchronous event handler for TEvent. This only called when calling SaveChanges of DbContext
/// </summary>
/// <typeparam name="TEvent">An implementation of <see cref="IEventItem" /></typeparam>
public interface IBeforeSaveEventHandlerAsync<in TEvent> : IEventHandlerAsync<TEvent> where TEvent : IEventItem
{
}