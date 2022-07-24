using System.Linq.Expressions;
using HBDStack.EfCore.Abstractions.QueryBuilders;
using Microsoft.EntityFrameworkCore;

namespace HBDStack.EfCore.Repos;

public static class RepoExtensions
{
    #region Methods

    public static async ValueTask<List<TEntity>> ToListAsync<TEntity>(this IAsyncEnumerable<TEntity> entities)
    {
        if (entities is null) throw new ArgumentNullException(nameof(entities));

        var list = new List<TEntity>();

        await foreach (var item in entities)
            list.Add(item);

        return list;
    }

    internal static IQueryable<T> ApplyFilterAndOrder<T>(this IQueryable<T> query, Expression<Func<T, bool>>? filter, IQueryBuilder<T>? ordering) where T : class
    {
        if (filter != null) query = query.Where(filter);
        if (ordering != null) query = ordering.Build(query);

        return query;
    }

    internal static IQueryable<T> ApplyFilterAndOrder<T, TKey>(this IQueryable<T> query, Expression<Func<T, bool>>? filter, Expression<Func<T, TKey>>? ordering) where T : class
    {
        if (filter != null) query = query.Where(filter);
        if (ordering != null) query = query.OrderBy(ordering);

        return query;
    }

    // internal static IQueryable<T> ApplySpecAndOrder<T>(this IQueryable<T> query, Spec<T> spec, IQueryBuilder<T>? ordering) where T : class
    // {
    //     query = query.Wheres(spec);
    //     if (ordering != null) query = ordering.Build(query);
    //
    //     return query;
    // }
    //
    // internal static IQueryable<T> ApplySpecAndOrder<T, TKey>(this IQueryable<T> query, Spec<T> spec, Expression<Func<T, TKey>>? ordering) where T : class
    // {
    //     query = query.Wheres(spec);
    //     if (ordering != null) query = query.OrderBy(ordering);
    //
    //     return query;
    // }

    internal static IQueryable<T> TrackingChanges<T>(this IQueryable<T> query, bool tracking) where T : class
        => tracking ? query.AsTracking() : query.AsNoTracking();

    #endregion Methods
}