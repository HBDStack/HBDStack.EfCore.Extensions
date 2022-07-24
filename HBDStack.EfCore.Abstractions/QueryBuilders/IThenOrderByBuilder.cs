using System.Linq.Expressions;

namespace HBDStack.EfCore.Abstractions.QueryBuilders;

public interface IThenOrderByBuilder<T> : IQueryBuilder<T> where T : class
{
    #region Methods

    IThenOrderByBuilder<T> ThenBy(Expression<Func<T, object>> orderBy);

    IThenOrderByBuilder<T> ThenBy(string orderBy);

    IThenOrderByBuilder<T> ThenByDescending(Expression<Func<T, object>> orderBy);

    IThenOrderByBuilder<T> ThenByDescending(string orderBy);

    #endregion Methods
}