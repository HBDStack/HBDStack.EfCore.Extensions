namespace HBDStack.EfCore.Events.MiddleWare;

public interface IEventMapper
{
    object Map(object source, Type sourceType, Type destinationType);
}