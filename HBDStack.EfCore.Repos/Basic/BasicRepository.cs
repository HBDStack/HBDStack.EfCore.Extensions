using HBDStack.EfCore.Abstractions.Entities;
using HBDStack.EfCore.Repos.Abstractions.Basic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

// ReSharper disable MemberCanBePrivate.Global

namespace HBDStack.EfCore.Repos.Basic;

public class BasicRepository : BasicReadOnlyRepository, IBasicRepository
{
    public BasicRepository(DbContext dbContext) : base(dbContext, RepoTypes.ReadWrite) => DbContext = dbContext;

    protected internal DbContext DbContext { get; }

    public void Add<TEntity>(TEntity entity)
        where TEntity : class
        =>  DbContext.Add(entity);
    
    public async ValueTask AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
        where TEntity : class
        => await DbContext.AddAsync(entity, cancellationToken).ConfigureAwait(false);

    public Task ReloadAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
        where TEntity : class
        => DbContext.Entry(entity).ReloadAsync(cancellationToken);

    public void UpdateRowVersion<TEntity>(TEntity entity, byte[] rowVersion) where TEntity : class, IConcurrencyEntity 
        => DbContext.Entry(entity).Property(nameof(IConcurrencyEntity.RowVersion)).OriginalValue = rowVersion;

    public async Task<TEntity?> DeleteByIdAsync<TEntity>(params object[] id) where TEntity : class
    {
        var entity = await DbContext.FindAsync<TEntity>(id).ConfigureAwait(false);
        Delete(entity);
        return entity;
    }

    public bool Delete<TEntity>(TEntity? entity) where TEntity : class
    {
        if (entity == null) return false;
        DbContext.Remove(entity);
        return true;
    }

    public async ValueTask<int> SaveAsync(CancellationToken cancellationToken = default)
        => await DbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default) => DbContext.Database.BeginTransactionAsync(cancellationToken);

    /// <summary>
    ///     Update Entity. To improve performance Only call DbContext.Update if entity is Detached as this method will mark all
    ///     properties are modified.
    /// </summary>
    /// <param name="entity"></param>
    public void Update<TEntity>(TEntity entity) where TEntity : class
    {
        if (DbContext.Entry(entity).State == EntityState.Detached)
            DbContext.Update(entity);
    }
}