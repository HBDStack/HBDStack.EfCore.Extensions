using System.Collections.Immutable;
using System.Reflection;
using AutoMapper;
using HBDStack.EfCore.Abstractions.Events;
using HBDStack.EfCore.Hooks;
using HBDStack.EfCore.Events.Attributes;
using Microsoft.EntityFrameworkCore;
using StatusGeneric;

namespace HBDStack.EfCore.Events.Internals;

internal sealed class EventRunnerHook : /*IHook,*/ IHookAsync
{
    //private static readonly Type InternalRunner = typeof(EventHandlerRunner<>);
    private static readonly Type InternalAsyncRunner = typeof(EventHandlerRunnerAsync<>);
    private readonly EventHandlerFinder _eventHandlerFinder;
    private readonly IMapper _autoMapper;
    //private ImmutableList<EntityEventItem>? _currentEvents;
    private ImmutableList<EntityEventItem>? _currentAsyncEvents;

    public EventRunnerHook(EventHandlerFinder eventHandlerFinder, IMapper autoMapper)
    {
        _eventHandlerFinder = eventHandlerFinder;
        _autoMapper = autoMapper;
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
        _currentAsyncEvents = ProcessAndFilterDomainEvents(context);

        //Run Before Events
        var status = await RunEventsAsync(HandlerTypes.BeforeAsync, _currentAsyncEvents, cancellationToken)
            .ConfigureAwait(false);
        if (status.HasErrors) throw new EventException(status);
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

    private ImmutableList<EntityEventItem> ProcessAndFilterDomainEvents(SnapshotContext context)
    {
        var found = context.SnapshotEntities.Where(e => e.Entity is IEventEntity)
            .Select(e =>
            {
                var events = new List<IEventItem?>();
                var t = (IEventEntity)e.Entity;
                var eventsAndTypes = t.GetEventsAndClear();

                if (eventsAndTypes.events != null)
                {
                    //Collect events
                    events.AddRange(eventsAndTypes.events);
                }

                if (eventsAndTypes.eventTypes != null)
                {
                    //Collect events from Type
                    events.AddRange(eventsAndTypes.eventTypes.Select(type => (IEventItem)_autoMapper.Map(t, e.Entry.Metadata.ClrType, type)));
                }

                //Collect Auto events from Attribute.
                var autoEventAtt = e.Entry.Metadata.ClrType.GetCustomAttribute<AutoEventsAttribute>();

                if(autoEventAtt?.CreatedEventType != null && e.OriginalState == EntityState.Added)
                    events.Add((IEventItem)_autoMapper.Map(t,e.Entry.Metadata.ClrType,autoEventAtt.CreatedEventType));
                
                if(autoEventAtt?.UpdatedEventType != null && e.OriginalState == EntityState.Modified)
                    events.Add((IEventItem)_autoMapper.Map(t,e.Entry.Metadata.ClrType,autoEventAtt.UpdatedEventType));

                if(autoEventAtt?.DeletedEventType != null && e.OriginalState == EntityState.Deleted)
                    events.Add((IEventItem)_autoMapper.Map(t,e.Entry.Metadata.ClrType,autoEventAtt.DeletedEventType));
                
                return new EntityEventItem(t, events.Where(i=>i is not null).Distinct().ToArray()!);
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