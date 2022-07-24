// ReSharper disable CheckNamespace

using System.Linq.Expressions;
using HBDStack.EfCore.Abstractions.QueryBuilders;

namespace HBD.EfCore.Repos.Basic;

public interface IBasicReadOnlyRepository
{
    /// <summary>
    /// Get IQueryable of a Entity
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    public IQueryable<TEntity> Get<TEntity>() where TEntity : class;
        
    public ValueTask<List<TEntity>> GetAsync<TEntity>(Expression<Func<TEntity, bool>> filter,
        IQueryBuilder<TEntity>? ordering = null, CancellationToken cancellationToken = default) where TEntity : class;

    public ValueTask<List<TEntity>> GetAsync<TEntity, TKey>(Expression<Func<TEntity, bool>> filter,
        Expression<Func<TEntity, TKey>>? ordering = null, CancellationToken cancellationToken = default) where TEntity : class;

    public ValueTask<TEntity?> FindAsync<TEntity>(params object[] id) where TEntity : class;

    public ValueTask<TEntity?> FindAsync<TEntity>(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default) where TEntity : class;
}