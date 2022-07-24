using System.Linq.Expressions;

namespace HBDStack.EfCore.Abstractions.QueryBuilders;

public static class OrderBuilder
{

    public static IThenOrderByBuilder<T> CreateBuilder<T>(Expression<Func<T, object>> orderBy) where T : class => CreateBuilder<T>().OrderBy(orderBy);

    public static IThenOrderByBuilder<T> CreateBuilder<T>(string orderBy) where T : class => CreateBuilder<T>().OrderBy(orderBy);

    public static IQueryable<T> OrderWith<T>(this IQueryable<T> query, IQueryBuilder<T> order) where T : class
    {
        if (order is null)
            throw new ArgumentNullException(nameof(order));

        return order.Build(query);
    }

    internal static IOrderByBuilder<T> CreateBuilder<T>() where T : class => new OrderByBuilder<T>();
}