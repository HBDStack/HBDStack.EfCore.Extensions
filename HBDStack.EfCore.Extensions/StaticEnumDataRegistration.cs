using System.ComponentModel.DataAnnotations;
using System.Reflection;
using HBDStack.EfCore.Abstractions.Attributes;
using HBDStack.EfCore.Abstractions.Entities;
using HBDStack.EfCore.Extensions.Internal;
using HBDStack.EfCore.Extensions.Options;
using HBDStack.Framework.Extensions;
using Microsoft.EntityFrameworkCore;

namespace HBDStack.EfCore.Extensions;

internal static class StaticEnumDataRegistration
{
    
    private static readonly MethodInfo Method = typeof(StaticEnumDataRegistration)
        .GetMethod(nameof(RegisterStaticDataType), BindingFlags.Static | BindingFlags.NonPublic)!;

    /// <summary>
    ///     Register StaticDataOfAttribute Entities from RegistrationInfos <see cref="RegistrationInfo" />
    /// </summary>
    /// <param name="modelBuilder"></param>
    /// <param name="registrations"></param>
    internal static void RegisterStaticDataFrom(this ModelBuilder modelBuilder,
        IEnumerable<RegistrationInfo> registrations)
    {
        foreach (var type in registrations.SelectMany(r =>
                     r.EntityAssemblies.Extract().Enum().HasAttribute<StaticDataAttribute>()))
            modelBuilder.RegisterStaticDataFromEnumType(type);
    }

    private static void RegisterStaticDataFromEnumType(this ModelBuilder modelBuilder, Type enumType)
    {
        var att = enumType.GetCustomAttribute<StaticDataAttribute>();
        if (att == null) return;
        
        var enumTableType = typeof(EnumTables<>).MakeGenericType(enumType);

        if (Method == null)
            throw new ArgumentException($"The {nameof(RegisterStaticDataType)} is not found");

        var md = Method.MakeGenericMethod(enumTableType);
        md.Invoke(null, new object[] {modelBuilder, att});
    }

    private static void RegisterStaticDataType<TEnumTable>(ModelBuilder modelBuilder, StaticDataAttribute attribute)
        where TEnumTable : class
    {
        var enumType = typeof(TEnumTable).GetGenericArguments().First();
        var builder = modelBuilder.Entity<TEnumTable>()
            .ToTable(attribute?.Name ?? enumType.Name, attribute?.Schema);

        var columnsCreated = false;
        var hasDisplayAttribute = false;

        foreach (var value in Enum.GetValues(enumType))
        {
            if(value == null)continue;
            var displayAtt = enumType.GetMember(value.ToString()!)[0].GetCustomAttribute<DisplayAttribute>();

            if (!columnsCreated)
            {
                builder.Property<int>(nameof(IEntity<int>.Id))
                    .IsRequired()
                    .ValueGeneratedNever();

                builder.HasKey(nameof(IEntity<int>.Id));

                builder.Property<string>(nameof(DisplayAttribute.Name))
                    .HasMaxLength(100)
                    .IsRequired();

                if (displayAtt != null)
                {
                    builder.Property<string>(nameof(DisplayAttribute.Description))
                        .HasMaxLength(255);

                    builder.Property<string>(nameof(DisplayAttribute.GroupName))
                        .HasMaxLength(255);

                    hasDisplayAttribute = true;
                }

                columnsCreated = true;
            }

            if (hasDisplayAttribute)
                builder.HasData(new
                {
                    Id = (int) value,
                    Name = displayAtt?.Name ?? value.ToString(),
                    displayAtt?.Description,
                    displayAtt?.GroupName
                });
            else
                builder.HasData(new
                {
                    Id = (int) value,
                    Name = value.ToString()
                });
        }
    }
}