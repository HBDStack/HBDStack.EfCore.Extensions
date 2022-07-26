using HBDStack.EfCore.Abstractions.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace HBDStack.EfCore.Repos.Abstractions.Basic;

public interface IBasicRepository : IBasicReadOnlyRepository
{
    ValueTask AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : class;

    void Update<TEntity>(TEntity entity) where TEntity : class;

    ValueTask<int> UpdateAndSaveAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
        where TEntity : class
    {
        Update(entity);
        return SaveAsync(cancellationToken);
    }

    /// <summary>
    /// Reload the data from database.
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    Task ReloadAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : class;

    void UpdateRowVersion<TEntity>(TEntity entity, byte[] rowVersion) where TEntity : class, IConcurrencyEntity;

    Task<TEntity?> DeleteByIdAsync<TEntity>(params object[] id) where TEntity : class;

    bool Delete<TEntity>(TEntity? entity) where TEntity : class;

    ValueTask<int> SaveAsync(CancellationToken cancellationToken = default);
    
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
}