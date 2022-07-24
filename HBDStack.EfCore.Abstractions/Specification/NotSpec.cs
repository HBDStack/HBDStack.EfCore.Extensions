using System.Linq.Expressions;

namespace HBD.EfCore.Abstractions.Specification;

/// <inheritdoc/>
internal sealed class NotSpec<T> : Spec<T>
{
    #region Fields

    private readonly Spec<T> _specification;

    #endregion Fields

    #region Constructors

    public NotSpec(Spec<T> specification)
    {
        _specification = specification;
    }

    #endregion Constructors

    #region Methods

    public override IQueryable<T> Includes(IQueryable<T> query)
    {
        return _specification.Includes(query);
    }

    public override Expression<Func<T, bool>> ToExpression()
    {
        var expression = _specification.ToExpression();
        return expression.NotMe();
    }

    #endregion Methods
}