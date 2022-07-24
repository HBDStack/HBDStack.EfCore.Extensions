using System.Linq.Expressions;

namespace HBDStack.EfCore.Abstractions.QueryBuilders;

public enum OrderingDirection
{
    Asc = 1,
    Desc = 2
}

internal sealed class OrderInfo<T> where T : class
{
    public OrderingDirection Direction { get; set; } = OrderingDirection.Asc;

    public Expression<Func<T, object>>? OrderProperty { get; set; }

    public string? OrderPropertyString { get; set; }
}