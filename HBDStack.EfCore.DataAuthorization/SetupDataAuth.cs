using System.Diagnostics;
using HBDStack.EfCore.DataAuthorization;
using HBDStack.EfCore.Hooks;
using Microsoft.EntityFrameworkCore;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class SetupDataAuth
{
    /// <summary>
    ///     Add Custom Data Key Provider
    /// </summary>
    /// <param name="services"></param>
    /// <param name="lifetime"></param>
    /// <typeparam name="TProvider">The implementation of <see cref="IDataKeyProvider" /></typeparam>
    /// <returns></returns>
    public static IServiceCollection AddDataKeyProvider<TProvider>(this IServiceCollection services,
        ServiceLifetime lifetime = ServiceLifetime.Scoped)
        where TProvider : class, IDataKeyProvider
    {
        if (services.Any(s => s.ServiceType == typeof(DataKeyHook)))
        {
            Trace.WriteLine("DataKeyHook is already registered.");
            return services;
        }

        return services
            .AddGlobalModelBuilderRegister<DataKeyAuthQueryRegister>()
            .Add<IDataKeyProvider, TProvider>(lifetime)
            .AddHook<DataKeyHook>();
    }
}