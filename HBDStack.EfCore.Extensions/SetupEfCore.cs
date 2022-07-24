using HBDStack.EfCore.Extensions.Internal;
using HBDStack.EfCore.Extensions.Options;
using HBDStack.EfCore.Extensions.Services;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable CheckNamespace
namespace Microsoft.EntityFrameworkCore;

public static class SetupEfCore
{
   internal static readonly IList<Type> GlobalQueryFilters = new List<Type>();

    public static IServiceCollection AddGlobalModelBuilderRegister<TImplementation>(this IServiceCollection services)
        where TImplementation : class, IGlobalModelBuilderRegister
    {
        GlobalQueryFilters.Add(typeof(TImplementation));
        return services;
    }

    /// <summary>
    ///  Scan and register all Entities from assemblies to DbContext.
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    /// <param name="this"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static DbContextOptionsBuilder<TContext> UseAutoConfigModel<TContext>(
        this DbContextOptionsBuilder<TContext> @this, Action<IEntityMappingExtension>? options = null)
        where TContext : DbContext
    {
        return (DbContextOptionsBuilder<TContext>) ((DbContextOptionsBuilder) @this)
            .UseAutoConfigModel(options);
    }

    /// <summary>
    /// Scan and register all Entities from assemblies to DbContext.
    /// </summary>
    /// <param name="this"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static DbContextOptionsBuilder UseAutoConfigModel(this DbContextOptionsBuilder @this,
        Action<IEntityMappingExtension>? options = null)
    {
        if (@this is null)
            throw new ArgumentNullException(nameof(@this));

        var op = @this.GetOrCreateExtension();
        options?.Invoke(op);

        return @this;
    }

    private static EntityMappingExtension GetOrCreateExtension(this DbContextOptionsBuilder optionsBuilder)
    {
        var op = optionsBuilder.Options.FindExtension<EntityMappingExtension>();
        if (op != null) return op;

        op = new EntityMappingExtension();
        ((IDbContextOptionsBuilderInfrastructure) optionsBuilder).AddOrUpdateExtension(op);

        return op;
    }
}