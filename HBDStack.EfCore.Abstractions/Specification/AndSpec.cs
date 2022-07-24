using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace HBD.EfCore.Abstractions.Specification;

internal sealed class AndSpec<T> : Spec<T>
{
    #region Fields

    private readonly Spec<T> _left;
    private readonly Spec<T> _right;

    #endregion Fields

    #region Constructors

    public AndSpec(Spec<T> left, Spec<T> right)
    {
        _right = right;
        _left = left;
    }

    #endregion Constructors

    #region Methods

    public override IQueryable<T> Includes(IQueryable<T> query)
    {
        return query.Includes(_left).Includes(_right);
    }

    public override Expression<Func<T, bool>> ToExpression()
    {
        var leftExpression = _left.ToExpression();
        var rightExpression = _right.ToExpression();

        return leftExpression.And(rightExpression);
    }

    #endregion Methods
}