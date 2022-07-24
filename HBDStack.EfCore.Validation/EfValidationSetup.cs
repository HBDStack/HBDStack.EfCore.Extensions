// ReSharper disable CheckNamespace

using HBDStack.EfCore.Validation;

namespace Microsoft.Extensions.DependencyInjection;

public static class EfValidationSetup
{
    public static IServiceCollection AddEntityValidator<TImplement>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped) where TImplement : class, IEntityValidator
    {
        //This ensure only validation is added.
        return services.Replace<IEntityValidator, TImplement>(lifetime)
            .AsImplementedInterfaces(new[] { typeof(EntityValidationHook) }, lifetime);
    }

    public static IServiceCollection AddDefaultEntityValidator(this IServiceCollection services,
        ServiceLifetime lifetime = ServiceLifetime.Scoped)
        => services.AddEntityValidator<DefaultEntityValidator>(lifetime);
}