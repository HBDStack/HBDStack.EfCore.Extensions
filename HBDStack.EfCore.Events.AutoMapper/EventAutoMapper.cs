using AutoMapper;
using HBDStack.EfCore.Events.MiddleWare;

namespace HBDStack.EfCore.Events;

internal class EventAutoMapper:IEventMapper
{
    private readonly IMapper _mapper;
    public EventAutoMapper(IMapper mapper) => _mapper = mapper;

    public object Map(object source, Type sourceType, Type destinationType) => _mapper.Map(source, sourceType, destinationType);
}