using HBDStack.EfCore.Events;

namespace Microsoft.Extensions.DependencyInjection;

public static class SetupEventAutoMapper
{
    public static IServiceCollection AddEventAutoMapper(this IServiceCollection services) => services.AddEventMapper<EventAutoMapper>();
}