namespace HBDStack.EfCore.Abstractions;

public static class SharedExtensions
{
    ///NOTE as Ef5 and Ef6 already fixed owned property issues. So this method is no longer needed
    /// <summary>
    ///     Update the values from obj for Owned type ONLY. Those properties marked as
    ///     [ReadOnly(true)] or
    ///     TODO: Should improve the performance.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="this"></param>
    /// <param name="entity"></param>
    /// <param name="ignoreNull">
    ///     if true the Key property wont be updated. This is not true for composite key. Please use
    ///     <see cref="IgnoreFromUpdateAttribute" /> instead.
    /// </param>
    /// <param name="ignoreKey"></param>
    /// <param name="bindingFlags"></param>
    // public static T UpdateFrom<T>(this T @this, T entity,
    //     bool ignoreNull = false,
    //     bool ignoreKey = true,
    //     BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
    //     where T : class
    // {
    //     if (@this == null)
    //         throw new ArgumentNullException(nameof(@this));
    //     if (entity == null) throw new ArgumentNullException(nameof(entity));
    //
    //     if (typeof(T).GetCustomAttribute<OwnedAttribute>() == null)
    //         throw new ArgumentException("Only Owned Type is accepted");
    //
    //     #region Update Props
    //
    //     foreach (var property in entity.GetType().GetProperties(bindingFlags))
    //     {
    //         if (!property.CanRead || !property.CanWrite)
    //         {
    //             Trace.TraceInformation($"UpdateFrom: ignored not readable {property.Name}.");
    //             continue;
    //         }
    //
    //         var readOnly = property.GetCustomAttribute<ReadOnlyAttribute>();
    //         if (readOnly?.IsReadOnly == true)
    //         {
    //             Trace.TraceInformation($"UpdateFrom: ignored ReadOnly {property.Name}");
    //             continue;
    //         }
    //
    //         if (ignoreKey && property.GetCustomAttribute<KeyAttribute>() != null)
    //         {
    //             Trace.TraceInformation($"UpdateFrom: KeyAttribute {property.Name}.");
    //             continue;
    //         }
    //
    //         var ignored = property.GetCustomAttribute<IgnoreFromUpdateAttribute>();
    //         if (ignored != null)
    //         {
    //             Trace.TraceInformation($"UpdateFrom: IgnoreFromUpdate {property.Name}.");
    //             continue;
    //         }
    //
    //         var val = property.GetValue(entity);
    //
    //         if (val == null)
    //         {
    //             if (ignoreNull)
    //                 continue;
    //
    //             property.SetValue(@this, null);
    //             continue;
    //         }
    //
    //         property.SetValue(@this, val);
    //     }
    //
    //     #endregion Update Props
    //
    //     #region UpdateFields
    //
    //     foreach (var field in entity.GetType().GetFields(bindingFlags)
    //         .Where(f => f.GetCustomAttribute<CompilerGeneratedAttribute>() == null))
    //     {
    //         var ignored = field.GetCustomAttribute<IgnoreFromUpdateAttribute>();
    //         if (ignored != null)
    //         {
    //             Trace.TraceInformation($"UpdateFrom: IgnoreFromUpdate {field.Name}.");
    //             continue;
    //         }
    //
    //         var val = field.GetValue(entity);
    //
    //         if (val == null)
    //         {
    //             if (ignoreNull)
    //                 continue;
    //
    //             field.SetValue(@this, null);
    //             continue;
    //         }
    //
    //         field.SetValue(@this, val);
    //     }
    //
    //     #endregion UpdateFields
    //
    //     return @this;
    // }
}