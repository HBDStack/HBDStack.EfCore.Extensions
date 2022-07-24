using HBD.EfCore.Abstractions.Specification;

// ReSharper disable CheckNamespace
namespace Microsoft.EntityFrameworkCore;

public static class SpecExtensions
{
   /// <summary>
    ///     Add ONLY Including of Spec to query
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TSpec"></typeparam>
    /// <param name="this"></param>
    /// <returns></returns>
    public static IQueryable<T> Includes<T, TSpec>(this IQueryable<T> @this, params object[] args)
        where TSpec : Spec<T> =>
       @this.Includes(CreateInstance<T, TSpec>(args));

   /// <summary>
    ///     Add ONLY Including of Spec to query
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="this"></param>
    /// <param name="spec"></param>
    /// <returns></returns>
    public static IQueryable<T> Includes<T>(this IQueryable<T> @this, Spec<T>? spec=null) => spec == null ? @this : spec.Includes(@this);

   /// <summary>
   ///     Add ONLY expression condition of Spec to query
   /// </summary>
   /// <typeparam name="T"></typeparam>
   /// <typeparam name="TSpec"></typeparam>
   /// <param name="this"></param>
   /// <param name="args"></param>
   /// <returns></returns>
   public static IQueryable<T> Wheres<T, TSpec>(this IQueryable<T> @this, params object[] args)
        where TSpec : Spec<T> =>
       @this.Wheres(CreateInstance<T, TSpec>(args));

   /// <summary>
    ///     Add ONLY expression condition of Spec to query
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="this"></param>
    /// <param name="spec"></param>
    /// <returns></returns>
    public static IQueryable<T> Wheres<T>(this IQueryable<T> @this, Spec<T>? spec = null) => spec == null ? @this : @this.Where(spec.ToExpression());

   private static TSpec CreateInstance<T, TSpec>(params object[] args) where TSpec : Spec<T> => (TSpec) Activator.CreateInstance(typeof(TSpec), args)!;
}