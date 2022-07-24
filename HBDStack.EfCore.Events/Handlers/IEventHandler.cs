using HBDStack.EfCore.Abstractions.Events;
using StatusGeneric;

namespace HBDStack.EfCore.Events.Handlers;

/// <summary>
///     The Root Interface of Event handler.
///     DONOT use this to handle the event. Instead provide an implementation od
///     <see cref="IBeforeSaveEventHandler{TEvent}" /> or <see cref="IAfterSaveEventHandler{TEvent}" />
/// </summary>
/// <typeparam name="TEvent">
///     <see />
/// </typeparam>
// public interface IEventHandler<in TEvent> where TEvent : IEventItem
// {
//     IStatusGeneric Handle(IEventEntity callingEntity, TEvent domainEvent);
// }

/// <summary>
///     The Root Interface of Event handler async
///     DONOT use this to handle the event. Instead provide an implementation od
///     <see cref="IBeforeSaveEventHandlerAsync{TEvent}" /> or <see cref="IAfterSaveEventHandlerAsync{TEvent}" />
/// </summary>
/// <typeparam name="TEvent">
///     <see>
///         <cref>IDomainEvent</cref>
///     </see>
/// </typeparam>
public interface IEventHandlerAsync<in TEvent> where TEvent : IEventItem
{
    ValueTask<IStatusGeneric> HandleAsync(IEventEntity callingEntity, TEvent domainEvent,
        CancellationToken cancellationToken = default);
}