using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace HBD.EfCore.Abstractions.Specification;

/// <inheritdoc/>
internal sealed class OrSpec<T> : Spec<T>
{
    #region Fields

    private readonly Spec<T> _left;
    private readonly Spec<T> _right;

    #endregion Fields

    #region Constructors

    public OrSpec(Spec<T> left, Spec<T> right)
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
        var leftExp = _left.ToExpression();
        var rightExp = _right.ToExpression();
        var p = leftExp.Parameters.Single();

        return Expression.Lambda<Func<T, bool>>(
            Expression.Or(leftExp.Body, Expression.Invoke(rightExp, p)), p);
    }

    #endregion Methods
}