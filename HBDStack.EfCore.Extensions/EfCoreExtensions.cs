using HBDStack.EfCore.Abstractions.Attributes;
using HBDStack.EfCore.Extensions;
using Microsoft.EntityFrameworkCore.Infrastructure;

// ReSharper disable MemberCanBePrivate.Global

// ReSharper disable CheckNamespace
namespace Microsoft.EntityFrameworkCore;

public static class EfCoreExtensions
{
    #region Methods

    /// <summary>
    ///     Get Primary Keys of a Entity
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="dbContext"></param>
    /// <returns></returns>
    public static IEnumerable<string> GetKeys<TEntity>(this DbContext dbContext) => dbContext.GetKeys(typeof(TEntity));

    private static IEnumerable<string> GetKeys(this DbContext context, Type entityType)
    {
        if (context is null)
            throw new ArgumentNullException(nameof(context));

        return context.Model.FindEntityType(entityType)?.FindPrimaryKey()?.Properties.Select(i => i.Name)
               ?? Array.Empty<string>();
    }

    /// <summary>
    ///     Get Primary key value of a Entity
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="context"></param>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static IEnumerable<object?> GetKeyValuesOf<TEntity>(this DbContext context, object entity)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));

        var keys = context.GetKeys<TEntity>();

        foreach (var key in keys)
            yield return entity.GetType().GetProperty(key)?.GetValue(entity);
    }

    /// <summary>
    ///     Get Next Sequence value
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="dbContext"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static async ValueTask<TValue?> NextSeqValue<TEnum, TValue>(this DbContext dbContext, TEnum name)
        where TEnum : struct
        where TValue : struct =>
        (TValue?)await dbContext.NextSeqValue(name).ConfigureAwait(false);

    /// <summary>
    ///     Get Next Sequence value
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    /// <param name="dbContext"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static async ValueTask<object?> NextSeqValue<TEnum>(this DbContext dbContext, TEnum name)
        where TEnum : struct
    {
        if (dbContext is null)
            throw new ArgumentNullException(nameof(dbContext));

        var type = typeof(TEnum);
        var att = SequenceRegistration.GetAttribute(type);
        if (att == null) return null;

        await using var command = dbContext.Database.GetDbConnection().CreateCommand();
        command.CommandText = $"SELECT NEXT VALUE FOR {att.Schema}.{SequenceRegistration.GetSequenceName(name)}";

        await dbContext.Database.OpenConnectionAsync().ConfigureAwait(false);
        await using var result = await command.ExecuteReaderAsync().ConfigureAwait(false);
        
        object? rs = null;
        if (await result.ReadAsync().ConfigureAwait(false))
            rs = result.GetFieldValue<object>(0);

        await dbContext.Database.CloseConnectionAsync();
        if (rs == null)
            throw new InvalidOperationException(type.ToString());
        return rs;
    }

    /// <summary>
    ///     Get Next Sequence value with FormatString defined in <see cref="SequenceAttribute" />
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    /// <param name="dbContext"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static async ValueTask<string> NextSeqValueWithFormat<TEnum>(this DbContext dbContext, TEnum name)
        where TEnum : struct
    {
        var att = SequenceRegistration.GetFieldAttributeOrDefault(typeof(TEnum), name);
        var value = await dbContext.NextSeqValue(name).ConfigureAwait(false);

        if (string.IsNullOrEmpty(att.FormatString)) return $"{value}";

        var f = att.FormatString.Replace(nameof(DateTime), "0", StringComparison.OrdinalIgnoreCase);
        return string.Format(f, DateTime.Now, value);
    }

    internal static Type GetEntityType(Type entityMappingType)
        => entityMappingType.GetInterfaces().First(a => a.IsGenericType).GetGenericArguments().First();

    internal static bool IsSequenceSupported(this DatabaseFacade database)
        => database.ProviderName == "Microsoft.EntityFrameworkCore.SqlServer";

    #endregion Methods
}