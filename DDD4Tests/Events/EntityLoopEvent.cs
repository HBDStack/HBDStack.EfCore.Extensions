using HBDStack.EfCore.Abstractions.Events;

namespace DDD4Tests.Events;

public record EntityLoopEvent : IEventItem
{
}