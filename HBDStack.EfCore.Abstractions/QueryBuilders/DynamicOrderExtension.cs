using System.Linq.Expressions;
using System.Reflection;

#pragma warning disable CS8625

namespace HBDStack.EfCore.Abstractions.QueryBuilders;

public enum DynamicOrderMethods
{
    OrderBy,
    OrderByDescending,
    ThenBy,
    ThenByDescending
}

public record DynamicOrderInfo(string Property, OrderingDirection Direction);

public static class DynamicOrderExtension
{
    public static IOrderedQueryable<T> OrderByDescendingDynamic<T>(this IQueryable<T> source,
        IEnumerable<string> properties) =>
        properties.Aggregate<string, IOrderedQueryable<T>>(null,
            (current, p) =>
                current == null ? source.OrderByDescendingDynamic(p) : current.ThenByDescendingDynamic(p));

    public static IOrderedQueryable<T> OrderByDescendingDynamic<T>(this IQueryable<T> source, string property) =>
        ApplyOrder(source, property, DynamicOrderMethods.OrderByDescending);


    public static IOrderedQueryable<T> OrderByDynamic<T>(this IQueryable<T> source, IEnumerable<string> properties) =>
        properties.Aggregate<string, IOrderedQueryable<T>>(null,
            (current, p) => current == null ? source.OrderByDynamic(p) : current.ThenByDynamic(p));

    public static IOrderedQueryable<T> OrderByDynamic<T>(this IQueryable<T> source, string property) =>
        ApplyOrder(source, property, DynamicOrderMethods.OrderBy);

    /// <summary>
    /// Support multi order 
    /// </summary>
    /// <param name="source"></param>
    /// <param name="orders"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IOrderedQueryable<T> OrderByDynamics<T>(this IQueryable<T> source, params DynamicOrderInfo[] orders)
    {
        IOrderedQueryable<T>? result = null;

        foreach (var (property, orderingDirection) in orders)
        {
            if (result == null)
            {
                result = source.OrderByDynamic(property, orderingDirection);
                continue;
            }

            result = result.ThenByDynamic(property, orderingDirection);
        }

        ArgumentNullException.ThrowIfNull(result);
        return result;
    }

    public static IOrderedQueryable<T> OrderByDynamic<T>(this IQueryable<T> source, string property,
        OrderingDirection direction)
        => ApplyOrder(source, property,
            direction == OrderingDirection.Asc ? DynamicOrderMethods.OrderBy : DynamicOrderMethods.OrderByDescending);

    public static IOrderedQueryable<T> ThenByDynamic<T>(this IQueryable<T> source, string property,
        OrderingDirection direction) => ApplyOrder(source, property,
        direction == OrderingDirection.Asc ? DynamicOrderMethods.ThenBy : DynamicOrderMethods.ThenByDescending);

    public static IOrderedQueryable<T> ThenByDescendingDynamic<T>(this IOrderedQueryable<T> source, string property) =>
        ApplyOrder(source, property, DynamicOrderMethods.ThenByDescending);

    public static IOrderedQueryable<T> ThenByDynamic<T>(this IOrderedQueryable<T> source, string property) =>
        ApplyOrder(source, property, DynamicOrderMethods.ThenBy);

    private static IOrderedQueryable<T> ApplyOrder<T>(IQueryable<T> source, string property,
        DynamicOrderMethods methodName)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));
        var mName = methodName.ToString();

        var props = property.Split('.');
        var type = typeof(T);
        var arg = Expression.Parameter(type, "x");
        Expression expr = arg;

        //Get Property of Child Object and create expression OrderMethod(t=>t.Property)
        foreach (var prop in props)
        {
            // use reflection (not ComponentModel) to mirror LINQ
            var pi = type.GetProperty(prop, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (pi == null) throw new ArgumentException($"Property {prop} not found on type {type.Name}");

            expr = Expression.Property(expr, pi);
            type = pi.PropertyType;
        }

        //Create generic order method
        var delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
        var lambda = Expression.Lambda(delegateType, expr, arg);

        var result = typeof(Queryable).GetRuntimeMethods().First(
                method => method.Name.Equals(mName, StringComparison.OrdinalIgnoreCase)
                          && method.IsGenericMethodDefinition
                          && method.GetGenericArguments().Length == 2
                          && method.GetParameters().Length == 2)
            .MakeGenericMethod(typeof(T), type)
            .Invoke(null, new object[] { source, lambda });

        ArgumentNullException.ThrowIfNull(result);
        return (IOrderedQueryable<T>)result;
    }
}