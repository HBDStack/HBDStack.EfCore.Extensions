using Microsoft.Extensions.DependencyInjection;

namespace HBDStack.EfCore.Events.Internals;

internal class EventHandlerFinder
{
    private readonly IServiceProvider _provider;

    public EventHandlerFinder(IServiceProvider provider) => _provider = provider;

    public IEnumerable<HandlerAndEntityEvent> Find(HandlerTypes type, EntityEventItem entityEventObj)
    {
        var handlerType = GetHandlerType(type);

        foreach (var @event in entityEventObj.Events)
        {
            var eventType = @event.GetType();
            var genericHandlerType = handlerType.MakeGenericType(eventType);
            var handlers = _provider.GetServices(genericHandlerType).ToList();

            //Trace.TraceInformation("EventHandlerFinder: Found {0} Handlers for {1}", handlers.Count, eventType.Name);

            foreach (var h in handlers.Where(h => h != null))
                yield return new HandlerAndEntityEvent(type, h!, entityEventObj.Entity, @event);
        }
    }

    private static Type GetHandlerType(HandlerTypes type) =>
        type switch
        {
            //HandlerTypes.Before => EventsExtensions.BeforeType,
            HandlerTypes.BeforeAsync => EventsExtensions.BeforeAsyncType,
            //HandlerTypes.After => EventsExtensions.AfterType,
            HandlerTypes.AfterAsync => EventsExtensions.AfterAsyncType,
            _ => throw new NotSupportedException(type.ToString())
        };
}