using HBDStack.EfCore.Events.Handlers;

namespace HBDStack.EfCore.Events;

public static class EventsExtensions
{
    //internal static readonly Type BeforeType = typeof(IBeforeSaveEventHandler<>);
    internal static readonly Type BeforeAsyncType = typeof(IBeforeSaveEventHandlerAsync<>);
    //internal static readonly Type AfterType = typeof(IAfterSaveEventHandler<>);
    internal static readonly Type AfterAsyncType = typeof(IAfterSaveEventHandlerAsync<>);
}