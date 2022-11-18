using System.Collections.Immutable;
using System.Data;
using System.Reflection;
using HBDStack.EfCore.Abstractions.Events;
using HBDStack.EfCore.Events.Attributes;
using HBDStack.EfCore.Hooks;
using HBDStack.ObjectMapper.Abstraction;
using HBDStack.StatusGeneric;
using Microsoft.EntityFrameworkCore;

namespace HBDStack.EfCore.Events.Internals;

internal sealed class EventRunnerHook : /*IHook,*/ IHookAsync
{
    private const int MAX_SCAN_EVENTS_COUNT = 5;

    //private static readonly Type InternalRunner = typeof(EventHandlerRunner<>);
    private static readonly Type InternalAsyncRunner = typeof(EventHandlerRunnerAsync<>);
    private readonly EventHandlerFinder _eventHandlerFinder;

    private readonly IObjectMapper? _autoMapper;

    //private ImmutableList<EntityEventItem>? _currentEvents;
    private ImmutableList<EntityEventItem>? _currentAsyncEvents;

    public EventRunnerHook(EventHandlerFinder eventHandlerFinder, IEnumerable<IObjectMapper> autoMappers)
    {
        _eventHandlerFinder = eventHandlerFinder;
        _autoMapper = autoMappers.FirstOrDefault();
    }

    /// <summary>
    /// RunBeforeSave
    /// </summary>
    /// <param name="context"></param>
    /// <exception cref="EventException"></exception>
    // public void RunBeforeSave(SnapshotContext context)
    // {
    //     _currentEvents = ProcessAndFilterDomainEvents(context);
    //
    //     //Run Before Events
    //     var status = RunEvents(HandlerTypes.Before, _currentEvents);
    //     if (status.HasErrors) throw new EventException(status);
    // }

    /// <summary>
    /// Run After Events and ignore the result even failed.
    /// </summary>
    /// <param name="context"></param>
    // public void RunAfterSave(SnapshotContext context)
    // {
    //     RunEvents(HandlerTypes.After, _currentEvents);
    //     _currentEvents = null;
    // }

    /// <summary>
    /// RunBeforeSaveAsync
    /// </summary>
    /// <param name="context"></param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="EventException"></exception>
    public async Task RunBeforeSaveAsync(SnapshotContext context,
        CancellationToken cancellationToken = default)
    {
        _currentAsyncEvents = ImmutableList<EntityEventItem>.Empty;

        var scanCount = 0;
        do
        {
            var processingEvents = ProcessAndFilterDomainEvents(context, scanCount == 0);
            scanCount++;

            if (processingEvents.IsEmpty)
                return;

            _currentAsyncEvents = _currentAsyncEvents.AddRange(processingEvents);

            //Run Before Events
            var status = await RunEventsAsync(HandlerTypes.BeforeAsync, processingEvents, cancellationToken)
                .ConfigureAwait(false);
            if (status.HasErrors) throw new EventException(status);
        } while (scanCount < MAX_SCAN_EVENTS_COUNT);

        throw new OverflowException(
            $"Overflow maximum {MAX_SCAN_EVENTS_COUNT} " +
            "loop check for run domain event handler. " +
            "So many domain events handler which continue adding new event " +
            "which to be considering a code smell");
    }

    /// <summary>
    /// Run RunAfterSaveAsync Events and ignore the result even failed.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="cancellationToken"></param>
    public async Task RunAfterSaveAsync(SnapshotContext context, CancellationToken cancellationToken = default)
    {
        await RunEventsAsync(HandlerTypes.AfterAsync, _currentAsyncEvents, cancellationToken).ConfigureAwait(false);
        _currentAsyncEvents = null;
    }

    /// <summary>
    ///     Create instance of <see cref="EventHandlerRunner{TEvent}" /> or <see cref="EventHandlerRunnerAsync{TEvent}" />
    ///     based on info.Type.
    /// </summary>
    /// <param name="info"></param>
    /// <returns></returns>
    private static dynamic? CreateInternalRunner(HandlerAndEntityEvent info)
    {
        //var handlerType = info.IsAsyncType ? InternalAsyncRunner : InternalRunner;
        var handlerType = InternalAsyncRunner;
        return Activator.CreateInstance(handlerType.MakeGenericType(info.Event.GetType()), info);
    }

    private ImmutableList<EntityEventItem> ProcessAndFilterDomainEvents(SnapshotContext context,
        bool scanAutoEvents = true)
    {
        var found = context.SnapshotEntities.Where(e => e.Entity is IEventEntity)
            .Select(e =>
            {
                var events = new List<IEventItem?>();
                var finallyEventTypes = new List<Type>();
                var entity = (IEventEntity)e.Entity;
                var eventsAndTypes = entity.GetEventsAndClear();

                if (eventsAndTypes.events != null)
                {
                    //Collect events
                    events.AddRange(eventsAndTypes.events);
                }

                if (eventsAndTypes.eventTypes != null)
                    finallyEventTypes.AddRange(eventsAndTypes.eventTypes);

                if (scanAutoEvents)
                {
                    //Collect Auto events from Attribute.
                    var autoEventAtt = e.Entry.Metadata.ClrType.GetCustomAttribute<AutoEventsAttribute>();

                    if (autoEventAtt?.CreatedEventType != null && e.OriginalState == EntityState.Added)
                        finallyEventTypes.Add(autoEventAtt.CreatedEventType);

                    if (autoEventAtt?.UpdatedEventType != null && e.OriginalState == EntityState.Modified)
                        finallyEventTypes.Add(autoEventAtt.UpdatedEventType);

                    if (autoEventAtt?.DeletedEventType != null && e.OriginalState == EntityState.Deleted)
                        finallyEventTypes.Add(autoEventAtt.DeletedEventType);
                }

                if (finallyEventTypes.Any())
                {
                    if (_autoMapper == null)
                        throw new NoNullAllowedException($"The {nameof(IObjectMapper)} is not provided.");
                    events.AddRange(finallyEventTypes.Distinct()
                        .Select(d => (IEventItem)_autoMapper.Map(entity, e.Entry.Metadata.ClrType, d)));
                }

                return new EntityEventItem(entity, events.Where(i => i is not null).Distinct().ToArray()!);
            })
            .Where(e => e.Events.Count > 0)
            .ToImmutableList();

        return found;
    }

    // private IStatusGeneric RunEvents(HandlerTypes type, ImmutableList<EntityEventItem>? events)
    // {
    //     if (events == null || events.IsEmpty) return new StatusGenericHandler();
    //     var foundHandlers = events.SelectMany(e => _eventHandlerFinder.Find(type, e)).ToList();
    //
    //     Trace.TraceInformation("EventRunnerHook: Found {0} Handlers for type {1}", foundHandlers.Count, type);
    //
    //     // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
    //     foreach (var handler in foundHandlers)
    //     {
    //         var runner = (EventHandlerRunner)CreateInternalRunner(handler)!;
    //         var status = runner.Handle(handler.Entity, handler.Event);
    //
    //         if (status.HasErrors)
    //             return status;
    //     }
    //
    //     return new StatusGenericHandler();
    // }

    private async ValueTask<IStatusGeneric> RunEventsAsync(HandlerTypes type, ImmutableList<EntityEventItem>? events,
        CancellationToken cancellationToken = default)
    {
        if (events == null || events.IsEmpty) return new StatusGenericHandler();
        var foundHandlers = @events.SelectMany(e => _eventHandlerFinder.Find(type, e));

        foreach (var handler in foundHandlers)
        {
            var runner = (EventHandlerRunnerAsync)CreateInternalRunner(handler)!;
            var status = await runner.HandleAsync(handler.Entity, handler.Event, cancellationToken)
                .ConfigureAwait(false);

            if (status.HasErrors)
                return status;
        }

        return new StatusGenericHandler();
    }
}