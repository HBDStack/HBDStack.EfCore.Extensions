using System.Runtime.CompilerServices;
using HBD.EfCore.Repos;
using HBD.EfCore.Repos.Basic;
using HBDStack.EfCore.Repos;
using HBDStack.EfCore.Repos.Basic;
using Microsoft.EntityFrameworkCore;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class SetupRepository
{
    /// <summary>
    ///     Add Generic Repositories. All generic repository is required DbContext as constructor parameter.
    ///     Ensure you expose an instance of DbContext in <see cref="IServiceCollection" />.
    ///     - <see cref="IReadOnlyRepository{TEntity}" />
    ///     - <see cref="IRepository{TEntity}" />
    ///     - <see cref="IDtoRepository{TEntity}" />
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddGenericRepositories(this IServiceCollection services)
    {
        if (services.Any(s => s.ServiceType == typeof(IBasicReadOnlyRepository))) return services;

        return services
            .AddScoped<IBasicReadOnlyRepository, BasicReadOnlyRepository>()
            .AddScoped<IBasicRepository, BasicRepository>()
            .AddScoped(typeof(IReadOnlyRepository<>), typeof(ReadOnlyRepository<>))
            .AddScoped(typeof(IRepository<>), typeof(Repository<>))
            .AddScoped(typeof(IDtoRepository<>), typeof(DtoRepository<>));
    }

    /// <summary>
    ///     Expose TDbContext as DbContext to <see cref="IServiceCollection" /> and Add Generic Repositories
    ///     <see cref="AddGenericRepositories" />.
    /// </summary>
    /// <param name="services"></param>
    /// <typeparam name="TDbContext">
    ///     This DbContext will be exposed as default Context in dependency injection in order to use
    ///     Generic Repositories.
    /// </typeparam>
    /// <returns></returns>
    public static IServiceCollection AddGenericRepositories<TDbContext>(this IServiceCollection services)
        where TDbContext : DbContext
    {
        if (services.All(s => s.ServiceType != typeof(DbContext)))
        {
            //TODO: This will cause too many IServiceProvider issue.
            services.AddScoped<DbContext>(sp => sp.GetRequiredService<TDbContext>());
        }

        return services.AddGenericRepositories();
    }
}