using HBDStack.EfCore.Events.Mapster;

namespace Microsoft.Extensions.DependencyInjection;

public static class SetupEventMapster
{
    public static IServiceCollection AddEventAutoMapper(this IServiceCollection services) => services.AddEventMapper<EventMapster>();
}