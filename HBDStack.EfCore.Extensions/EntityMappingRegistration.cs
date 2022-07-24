using System.Diagnostics;
using System.Reflection;
using HBDStack.EfCore.Abstractions.Attributes;
using HBDStack.EfCore.Abstractions.Entities;
using HBDStack.EfCore.Extensions.Configurations;
using HBDStack.EfCore.Extensions.Options;
using HBDStack.Framework.Extensions;

// ReSharper disable CheckNamespace
namespace Microsoft.EntityFrameworkCore;

internal static class EntityMappingRegistration
{
    private static readonly MethodInfo Method = typeof(EntityMappingRegistration)
        .GetMethod(nameof(RegisterMapping), BindingFlags.Static | BindingFlags.NonPublic)!;

    /// <summary>
    ///     Register EntityTypeConfiguration from RegistrationInfos <see cref="RegistrationInfo" />
    /// </summary>
    /// <param name="modelBuilder"></param>
    /// <param name="registrations"></param>
    internal static void RegisterEntityMappingFrom(this ModelBuilder modelBuilder,
        IEnumerable<RegistrationInfo> registrations)
    {
        foreach (var type in registrations)
            modelBuilder.RegisterEntityMappingFrom(type);
    }

    /// <summary>
    ///     Create a Mapper for an entity type. The first generic type that match with entity type condition will be selected.
    /// </summary>
    /// <param name="entityType"></param>
    /// <param name="mapperTypes">The list generic type</param>
    /// <returns></returns>
    private static Type CreateMapperFromGeneric(Type entityType, IEnumerable<Type> mapperTypes)
    {
        foreach (var mapperType in mapperTypes)
        {
            //The generic type should have 1 GenericTypeParameters only
            var genericType = mapperType.GetTypeInfo().GenericTypeParameters.Single();

            if (genericType.GetGenericParameterConstraints().Any(c => c.IsAssignableFrom(entityType))
                || genericType.IsAssignableFrom(entityType)
                || genericType.BaseType?.IsAssignableFrom(entityType) == true)
                return mapperType.MakeGenericType(entityType);
        }

        throw new ArgumentException(
            $"There is no {typeof(IEntityTypeConfiguration<>).Name} found for {entityType.Name}");
    }

    /// <summary>
    ///     Get all Entity Classes and Abstract class without Interface or Generic
    /// </summary>
    /// <param name="registration"></param>
    /// <returns></returns>
    internal static IEnumerable<Type> GetAllEntityTypes(this RegistrationInfo registration) =>
        registration.EntityAssemblies.Extract().Class().NotGeneric().IsInstanceOf(typeof(IEntity<>))
            .Where(t => !t.HasAttribute<IgnoreEntityMapperAttribute>(true));

    /// <summary>
    ///     Get All IEntityTypeConfiguration for entities.
    /// </summary>
    /// <param name="registration"></param>
    /// <returns></returns>
    internal static IEnumerable<Type> GetDefinedMappers(this RegistrationInfo registration) =>
        registration.EntityAssemblies.Extract().Class().NotAbstract().NotGeneric()
            .IsInstanceOf(typeof(IEntityTypeConfiguration<>));

    /// <summary>
    ///     Get All generic IEntityTypeConfiguration that can be use for the un-mapped entities.
    /// </summary>
    /// <param name="registration"></param>
    /// <returns></returns>
    internal static IEnumerable<Type> GetGenericMappers(this RegistrationInfo registration) =>
        registration.EntityAssemblies.Extract().Generic().Class().NotAbstract()
            .IsInstanceOf(typeof(IEntityTypeConfiguration<>));

    private static void RegisterEntityMappingFrom(this ModelBuilder modelBuilder, RegistrationInfo registration)
    {
        //modelBuilder.RegisterMappingFromType(type);
        if (registration.DefaultEntityMapperTypes == null)
            registration.WithDefaultMappersType(typeof(EntityTypeConfiguration<>));

        var allDefinedMappers = registration.GetDefinedMappers().ToList();
        var entityTypes = registration.GetAllEntityTypes().ToList();

        Trace.TraceInformation($"Found entities: {string.Join("\n", entityTypes.Select(t => t.Name))}");

        var requiredEntityTypes = registration.Predicate == null
            ? entityTypes
            : entityTypes.Where(registration.Predicate.Compile()).ToList();

        var genericMappers =
            registration.GetGenericMappers().Concat(registration.DefaultEntityMapperTypes!).ToList();

        Trace.TraceInformation($"Found Generic configuration: {string.Join("\n", genericMappers.Select(t => t.Name))}");

        //Map Entities to ModelBuilder
        foreach (var entityType in requiredEntityTypes)
        {
            var mapper = allDefinedMappers.FirstOrDefault(m =>
                m.BaseType?.GenericTypeArguments.FirstOrDefault() == entityType);

            if (mapper == null && !entityType.IsAbstract)
                mapper = CreateMapperFromGeneric(entityType, genericMappers);

            if (mapper != null)
            {
                modelBuilder.RegisterMappingFromType(mapper);
                //Remove Added from the list
                allDefinedMappers.Remove(mapper);
            }
        }

        //Add remaining to ModelBuilder
        foreach (var type in allDefinedMappers) modelBuilder.RegisterMappingFromType(type);

        //Ignore Others
        // if (registration.IgnoreOtherEntities)
        //     foreach (var entityType in entityTypes.Except(requiredEntityTypes))
        //         modelBuilder.Ignore(entityType);
    }

    /// <summary>
    ///     Generic RegisterMapping.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TMapping"></typeparam>
    /// <param name="builder"></param>
    /// <returns></returns>
    private static ModelBuilder RegisterMapping<TEntity, TMapping>(this ModelBuilder builder)
        where TMapping : IEntityTypeConfiguration<TEntity>
        where TEntity : class
    {
        var mapper = (IEntityTypeConfiguration<TEntity>)Activator.CreateInstance(typeof(TMapping))!;
        builder.ApplyConfiguration(mapper);
        return builder;
    }

    private static void RegisterMappingFromType(this ModelBuilder modelBuilder, Type mapperType)
    {
        if (mapperType == null) throw new ArgumentNullException(nameof(mapperType));

        var eType = EfCoreExtensions.GetEntityType(mapperType);

        if (Method == null || eType == null)
            throw new ArgumentException($"The {nameof(RegisterMapping)} or EntityType are not found");

        var md = Method.MakeGenericMethod(eType, mapperType);
        md.Invoke(null, new object[] { modelBuilder });
    }
}