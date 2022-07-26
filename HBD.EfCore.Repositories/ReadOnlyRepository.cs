using System.Linq.Expressions;
using HBDStack.EfCore.Abstractions.QueryBuilders;
using HBDStack.EfCore.Repos.Abstractions;
using HBDStack.EfCore.Repos.Abstractions.Basic;

// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable MemberCanBePrivate.Global

namespace HBDStack.EfCore.Repos;

[Flags]
public enum RepoTypes
{
    ReadOnly = 0,
    ReadWrite = 1
}

public class ReadOnlyRepository<TEntity> : IReadOnlyRepository<TEntity> where TEntity : class
{
    private readonly IBasicReadOnlyRepository _repository;
    public ReadOnlyRepository(IBasicReadOnlyRepository repository) => _repository = repository;
        
    /// <summary>
    ///     Get ReadOnly (No Tracking) Query for Entity
    /// </summary>
    /// <returns></returns>
    public virtual IQueryable<TEntity> Get() => _repository.Get<TEntity>();
        
    public virtual async ValueTask<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter,
        IQueryBuilder<TEntity>? ordering = null, CancellationToken cancellationToken = default) =>
        await _repository.GetAsync(filter, ordering, cancellationToken).ConfigureAwait(false);

    public virtual async ValueTask<List<TEntity>> GetAsync<TKey>(Expression<Func<TEntity, bool>> filter,
        Expression<Func<TEntity, TKey>>? ordering = null, CancellationToken cancellationToken = default) =>
        await _repository.GetAsync(filter, ordering, cancellationToken).ConfigureAwait(false);

    public virtual ValueTask<TEntity?> FindAsync(params object[] id)
        => _repository.FindAsync<TEntity>(id);

    public ValueTask<TEntity?> FindAsync(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default)
        => _repository.FindAsync(filter, cancellationToken);
}