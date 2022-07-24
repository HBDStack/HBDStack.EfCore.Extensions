using System.Linq.Expressions;

namespace HBD.EfCore.Abstractions.Specification;

/// <inheritdoc />
internal sealed class TrueSpec<T> : Spec<T>
{
    #region Methods

    public override Expression<Func<T, bool>> ToExpression() => x => true;

    #endregion Methods
}