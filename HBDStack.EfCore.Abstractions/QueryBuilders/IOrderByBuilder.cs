using System.Linq.Expressions;

namespace HBDStack.EfCore.Abstractions.QueryBuilders;

public interface IOrderByBuilder<T> : IQueryBuilder<T> where T : class
{
    #region Methods

    IThenOrderByBuilder<T> OrderBy(Expression<Func<T, object>> orderBy);

    IThenOrderByBuilder<T> OrderBy(string orderBy);

    IThenOrderByBuilder<T> OrderByDescending(Expression<Func<T, object>> orderBy);

    IThenOrderByBuilder<T> OrderByDescending(string orderBy);

    #endregion Methods
}