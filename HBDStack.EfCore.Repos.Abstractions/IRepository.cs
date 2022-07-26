using HBDStack.EfCore.Abstractions.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace HBDStack.EfCore.Repos.Abstractions;

public interface IRepository<TEntity> : IReadOnlyRepository<TEntity> where TEntity : class
{
    ValueTask AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task<TEntity?> DeleteByIdAsync(params object[] id);

    bool Delete(TEntity entity);
        
    ValueTask<int> SaveAsync(CancellationToken cancellationToken = default);

    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    
    void Update(TEntity entity);
    
    /// <summary>
    /// Update the 
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="rowVersion"></param>
    /// <typeparam name="TSEntity"></typeparam>
    void UpdateRowVersion<TSEntity>(TSEntity entity, byte[] rowVersion) where TSEntity : class, TEntity, IConcurrencyEntity;
    
    /// <summary>
    /// Update and Save changes
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    ValueTask<int> UpdateAndSaveAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        Update(entity);
        return SaveAsync(cancellationToken);
    }
    
    Task ReloadAsync(TEntity entity, CancellationToken cancellationToken = default);
}