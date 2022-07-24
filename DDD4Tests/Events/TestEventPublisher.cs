using System.Collections.Generic;
using System.Threading.Tasks;
using HBDStack.EfCore.Abstractions.Events;
using HBDStack.EfCore.Events.Handlers;

namespace DDD4Tests.Events;

public class TestEventPublisher : IEventPublisher
{
    public static ICollection<IEventItem> Events { get;  } = new List<IEventItem>();

    public Task PublishAsync(IEventItem domainEvent)
    {
        Events.Add(domainEvent);
        return Task.CompletedTask;
    }
}