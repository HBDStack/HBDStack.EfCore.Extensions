using HBDStack.EfCore.Abstractions.Entities;
using HBDStack.EfCore.Repos.Abstractions;
using HBDStack.EfCore.Repos.Abstractions.Basic;
using HBDStack.EfCore.Repos.Basic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

// ReSharper disable MemberCanBePrivate.Global

namespace HBDStack.EfCore.Repos;

public class Repository<TEntity> : ReadOnlyRepository<TEntity>, IRepository<TEntity> where TEntity : class
{
    private readonly IBasicRepository _repository;
    public Repository(IBasicRepository repository) : base(repository) => _repository = repository;

    protected internal DbContext? DbContext => (_repository as BasicRepository)?.DbContext;

    public ValueTask AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        => _repository.AddAsync(entity, cancellationToken);

    public Task<TEntity?> DeleteByIdAsync(params object[] id)
        => _repository.DeleteByIdAsync<TEntity>(id);

    public bool Delete(TEntity entity) => _repository.Delete(entity);

    public ValueTask<int> SaveAsync(CancellationToken cancellationToken = default)
        => _repository.SaveAsync(cancellationToken);

    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        => _repository.BeginTransactionAsync(cancellationToken);

    /// <summary>
    ///     Update Entity. To improve performance Only call DbContext.Update if entity is Detached as this method will mark all
    ///     properties are modified.
    /// </summary>
    /// <param name="entity"></param>
    public void Update(TEntity entity)
        => _repository.Update(entity);


    public void UpdateRowVersion<TSEntity>(TSEntity entity, byte[] rowVersion)
        where TSEntity : class, TEntity, IConcurrencyEntity
        => _repository.UpdateRowVersion(entity, rowVersion);

    public Task ReloadAsync(TEntity entity, CancellationToken cancellationToken = default)
        => _repository.ReloadAsync(entity, cancellationToken);
}