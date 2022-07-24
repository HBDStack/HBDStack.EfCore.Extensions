using System.Linq.Expressions;
using HBDStack.EfCore.Abstractions.QueryBuilders;

// ReSharper disable CheckNamespace

namespace HBD.EfCore.Repos;

public interface IReadOnlyRepository<TEntity> where TEntity : class
{
    /// <summary>
    /// Get Queryable of Entity
    /// </summary>
    /// <returns></returns>
    public IQueryable<TEntity> Get();

        
    public ValueTask<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter,
        IQueryBuilder<TEntity>? ordering = null, CancellationToken cancellationToken = default);

    public ValueTask<List<TEntity>> GetAsync<TKey>(Expression<Func<TEntity, bool>> filter,
        Expression<Func<TEntity, TKey>>? ordering = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Find entity by Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public ValueTask<TEntity?> FindAsync(params object[] id);

    /// <summary>
    /// Find first entity by condition
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public ValueTask<TEntity?> FindAsync(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default);
}