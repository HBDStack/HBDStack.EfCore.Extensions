using System.Linq.Expressions;
using HBDStack.EfCore.Abstractions.QueryBuilders;

namespace HBD.EfCore.Abstractions.Specification;

public class PageableSpec<T> : Spec<T>
{
    public PageableSpec(int pageIndex, int pageSize, Expression<Func<T, object>> orderBy, OrderingDirection direction = OrderingDirection.Asc, Spec<T>? spec = null)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
        OrderBy = orderBy ?? throw new ArgumentNullException(nameof(orderBy));
        OrderDirection = direction;
        InternalSpec = spec;
    }
    
    public Spec<T>? InternalSpec { get; }
    public Expression<Func<T, object>> OrderBy { get; }
    public OrderingDirection OrderDirection { get; }
    public int PageIndex { get; }
    public int PageSize { get; }


    public override IQueryable<T> Includes(IQueryable<T> query) => InternalSpec == null ? query : InternalSpec.Includes(query);

    public override Expression<Func<T, bool>> ToExpression() => InternalSpec == null ? All.ToExpression() : InternalSpec.ToExpression();
}