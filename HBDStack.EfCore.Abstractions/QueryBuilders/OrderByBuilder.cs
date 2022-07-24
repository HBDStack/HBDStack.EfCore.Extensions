using System.Linq.Expressions;

// ReSharper disable ClassWithVirtualMembersNeverInherited.Global

namespace HBDStack.EfCore.Abstractions.QueryBuilders;

/// <summary>
///     Implemented Query Builder
/// </summary>
/// <typeparam name="T"></typeparam>
internal class OrderByBuilder<T> : IOrderByBuilder<T>, IThenOrderByBuilder<T> where T : class
{
    internal OrderByBuilder()
    {
    }
    
    private OrderInfo<T>? OrderByProp { get; set; }

    private IList<OrderInfo<T>> ThenByProps { get; } = new List<OrderInfo<T>>();

    public virtual IQueryable<T> Build(IQueryable<T> query)
    {
        query = Apply(query, OrderByProp);
        return ThenByProps.Aggregate(query, (q, i) => Apply(q, i));
    }

    public IThenOrderByBuilder<T> OrderBy(Expression<Func<T, object>> orderBy)
    {
        OrderByProp = new OrderInfo<T>
        {
            Direction = OrderingDirection.Asc,
            OrderProperty = orderBy
        };

        return this;
    }

    public IThenOrderByBuilder<T> OrderBy(string orderBy)
    {
        OrderByProp = new OrderInfo<T>
        {
            Direction = OrderingDirection.Asc,
            OrderPropertyString = orderBy
        };

        return this;
    }

    public IThenOrderByBuilder<T> OrderByDescending(Expression<Func<T, object>> orderBy)
    {
        OrderByProp = new OrderInfo<T>
        {
            Direction = OrderingDirection.Desc,
            OrderProperty = orderBy
        };

        return this;
    }

    public IThenOrderByBuilder<T> OrderByDescending(string orderBy)
    {
        OrderByProp = new OrderInfo<T>
        {
            Direction = OrderingDirection.Desc,
            OrderPropertyString = orderBy
        };

        return this;
    }

    public IThenOrderByBuilder<T> ThenBy(Expression<Func<T, object>> orderBy)
    {
        ThenByProps.Add(new OrderInfo<T>
        {
            Direction = OrderingDirection.Asc,
            OrderProperty = orderBy
        });

        return this;
    }

    public IThenOrderByBuilder<T> ThenBy(string orderBy)
    {
        ThenByProps.Add(new OrderInfo<T>
        {
            Direction = OrderingDirection.Asc,
            OrderPropertyString = orderBy
        });

        return this;
    }

    public IThenOrderByBuilder<T> ThenByDescending(Expression<Func<T, object>> orderBy)
    {
        ThenByProps.Add(new OrderInfo<T>
        {
            Direction = OrderingDirection.Desc,
            OrderProperty = orderBy
        });

        return this;
    }

    public IThenOrderByBuilder<T> ThenByDescending(string orderBy)
    {
        ThenByProps.Add(new OrderInfo<T>
        {
            Direction = OrderingDirection.Desc,
            OrderPropertyString = orderBy
        });

        return this;
    }

    private static IQueryable<T> Apply(IQueryable<T> query, OrderInfo<T>? orderInfo)
    {
        if (orderInfo == null) return query;

        if (typeof(IOrderedQueryable<T>) == query.Expression.Type)
        {
            var orderedQuery = (IOrderedQueryable<T>) query;

            if (orderInfo.OrderProperty != null)
                return orderInfo.Direction == OrderingDirection.Asc
                    ? orderedQuery.ThenBy(orderInfo.OrderProperty)
                    : orderedQuery.ThenByDescending(orderInfo.OrderProperty);

            if (!string.IsNullOrEmpty(orderInfo.OrderPropertyString))
                return orderInfo.Direction == OrderingDirection.Asc
                    ? orderedQuery.ThenByDynamic(orderInfo.OrderPropertyString)
                    : orderedQuery.ThenByDescendingDynamic(orderInfo.OrderPropertyString);
        }
        else
        {
            if (orderInfo.OrderProperty != null)
                return orderInfo.Direction == OrderingDirection.Asc
                    ? query.OrderBy(orderInfo.OrderProperty)
                    : query.OrderByDescending(orderInfo.OrderProperty);

            if (!string.IsNullOrEmpty(orderInfo.OrderPropertyString))
                return orderInfo.Direction == OrderingDirection.Asc
                    ? query.OrderByDynamic(orderInfo.OrderPropertyString)
                    : query.OrderByDescendingDynamic(orderInfo.OrderPropertyString);
        }

        return query;
    }
}