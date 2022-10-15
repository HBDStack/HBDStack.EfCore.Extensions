using HBDStack.EfCore.Events.MiddleWare;
using MapsterMapper;

namespace HBDStack.EfCore.Events.Mapster;

internal class EventMapster:IEventMapper
{
    private readonly IMapper _mapper;

    public EventMapster(IMapper mapper) => _mapper = mapper;

    public object Map(object source, Type sourceType, Type destinationType) => _mapper.Map(source, sourceType, destinationType);
}