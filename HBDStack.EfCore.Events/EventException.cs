using HBDStack.StatusGeneric;

namespace HBDStack.EfCore.Events;

public class EventException : Exception
{
    public EventException(IStatusGeneric status) : base(status.Message) => Status = status;

    public IStatusGeneric Status { get; }
}