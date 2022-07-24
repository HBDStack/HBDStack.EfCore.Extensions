using System.Reflection;
using HBDStack.EfCore.Abstractions.Attributes;
using HBDStack.EfCore.Extensions.Options;
using HBDStack.Framework.Extensions;
using Microsoft.EntityFrameworkCore;

namespace HBDStack.EfCore.Extensions;

internal static class SequenceRegistration
{
    internal static SqlSequenceAttribute? GetAttribute(Type enumType) => enumType.GetCustomAttribute<SqlSequenceAttribute>();

    internal static SequenceAttribute GetFieldAttributeOrDefault(Type enumType, object field) =>
         enumType.GetMember(field.ToString()!)[0].GetCustomAttribute<SequenceAttribute>() ?? new SequenceAttribute();

    internal static string GetSequenceName(object field) => $"Sequence_{field}";

    /// <summary>
    ///     Register Sequence from Enums
    /// </summary>
    /// <param name="modelBuilder"></param>
    /// <param name="registrations"></param>
    internal static void RegisterSequencesFrom(this ModelBuilder modelBuilder,
        IEnumerable<RegistrationInfo> registrations)
    {
        foreach (var type in registrations.SelectMany(r =>
                     r.EntityAssemblies.Extract().Enum().HasAttribute<SqlSequenceAttribute>()))
            modelBuilder.RegisterSequencesFromEnumType(type);
    }

    private static void RegisterSequencesFromEnumType(this ModelBuilder modelBuilder, Type enumType)
    {
        var att = GetAttribute(enumType);
        if (att == null)
            throw new ArgumentException($"There is no SequenceEnumAttribute found for {enumType.Name}");

        var fields = Enum.GetValues(enumType);

        foreach (var f in fields)
        {
            var fieldAtt = GetFieldAttributeOrDefault(enumType, f);
            var name = GetSequenceName(f);

            var seq = modelBuilder.HasSequence(fieldAtt.Type, name, att.Schema);

            if (fieldAtt.StartAt > 0)
                seq.StartsAt(fieldAtt.StartAt);
            if (fieldAtt.IncrementsBy > 0)
                seq.IncrementsBy(fieldAtt.IncrementsBy);
            if (fieldAtt.Min > 0)
                seq.HasMin(fieldAtt.Min);
            if (fieldAtt.Max > 0)
                seq.HasMax(fieldAtt.Max);

            seq.IsCyclic(fieldAtt.Cyclic);
        }
    }
}