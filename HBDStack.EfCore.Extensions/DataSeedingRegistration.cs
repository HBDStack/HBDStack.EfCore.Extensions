using System.Reflection;
using HBDStack.EfCore.Extensions.Configurations;
using HBDStack.EfCore.Extensions.Options;
using HBDStack.Framework.Extensions;

// ReSharper disable CheckNamespace
namespace Microsoft.EntityFrameworkCore;

internal static class DataSeedingRegistration
{
    private static readonly MethodInfo Method = typeof(DataSeedingRegistration)
        .GetMethod(nameof(RegisterData), BindingFlags.Static | BindingFlags.NonPublic)!;
    
    /// <summary>
    ///     Register EntityTypeConfiguration from RegistrationInfos <see cref="RegistrationInfo" />
    /// </summary>
    /// <param name="modelBuilder"></param>
    /// <param name="registrations"></param>
    internal static void RegisterDataSeedingFrom(this ModelBuilder modelBuilder,
        IEnumerable<RegistrationInfo> registrations)
    {
        foreach (var type in registrations.SelectMany(GetDataSeedingTypes))
            modelBuilder.HasData(type);
    }

    private static Type[] GetDataSeedingTypes(this ICollection<Assembly> assemblies) =>
        assemblies.ToArray().Extract().Class().NotAbstract().NotGeneric().NotInterface()
            .IsInstanceOf(typeof(IDataSeedingConfiguration<>)).ToArray();

    private static Type[] GetDataSeedingTypes(this RegistrationInfo @this) => GetDataSeedingTypes(@this.EntityAssemblies);

    private static void HasData(this ModelBuilder modelBuilder, Type mapperType)
    {
        if (mapperType == null) throw new ArgumentNullException(nameof(mapperType));

        var eType = EfCoreExtensions.GetEntityType(mapperType);

        if (Method == null || eType == null)
            throw new ArgumentException($"The {nameof(RegisterData)} or EntityType are not found");

        var md = Method.MakeGenericMethod(eType, mapperType);
        md.Invoke(null, new object[] {modelBuilder});
    }

    private static ModelBuilder RegisterData<TEntity, TMapping>(this ModelBuilder builder)
        where TMapping : IDataSeedingConfiguration<TEntity>
        where TEntity : class
    {
        var dataSeeding = (IDataSeedingConfiguration<TEntity>) Activator.CreateInstance(typeof(TMapping))!;

        //dataSeeding.Apply(builder.Entity<TEntity>());
        builder.Entity<TEntity>().HasData(dataSeeding.Data.ToArray());
        return builder;
    }
}