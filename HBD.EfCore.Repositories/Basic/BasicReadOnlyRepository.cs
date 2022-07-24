using System.Linq.Expressions;
using HBD.EfCore.Repos.Basic;
using HBDStack.EfCore.Abstractions.QueryBuilders;
using Microsoft.EntityFrameworkCore;

// ReSharper disable MemberCanBePrivate.Global

namespace HBDStack.EfCore.Repos.Basic;

public class BasicReadOnlyRepository : IBasicReadOnlyRepository
{
    private readonly RepoTypes _repoTypes;

    private DbContext DbContext { get; }

    public BasicReadOnlyRepository(DbContext dbContext) : this(dbContext, RepoTypes.ReadOnly)
    {
    }

    protected BasicReadOnlyRepository(DbContext dbContext, RepoTypes repoTypes)
    {
        DbContext = dbContext;
        _repoTypes = repoTypes;
    }

    public virtual IQueryable<TEntity> Get<TEntity>() where TEntity : class => GetQuery<TEntity>(false);

    protected IQueryable<TEntity> GetQuery<TEntity>(bool ignoreFilters) where TEntity : class
    {
        var query = ignoreFilters ? DbContext.Set<TEntity>().IgnoreQueryFilters() : DbContext.Set<TEntity>();
        return query.TrackingChanges(_repoTypes == RepoTypes.ReadWrite);
    }

    protected virtual IQueryable<TEntity> GetQuery<TEntity>(Expression<Func<TEntity, bool>> filter, IQueryBuilder<TEntity>? ordering, bool ignoreFilters) where TEntity : class
    {
        var query = GetQuery<TEntity>(ignoreFilters);
        return query.ApplyFilterAndOrder(filter, ordering);
    }

    protected virtual IQueryable<TEntity> GetQuery<TEntity, TKey>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TKey>>? ordering, bool ignoreFilters) where TEntity : class
    {
        var query = GetQuery<TEntity>(ignoreFilters);
        return query.ApplyFilterAndOrder(filter, ordering);
    }


    public virtual async ValueTask<List<TEntity>> GetAsync<TEntity>(Expression<Func<TEntity, bool>> filter,
        IQueryBuilder<TEntity>? ordering = null, CancellationToken cancellationToken = default) where TEntity : class =>
        await GetQuery(filter, ordering, false).ToListAsync(cancellationToken: cancellationToken).ConfigureAwait(false);

    public virtual async ValueTask<List<TEntity>> GetAsync<TEntity, TKey>(Expression<Func<TEntity, bool>> filter,
        Expression<Func<TEntity, TKey>>? ordering = null, CancellationToken cancellationToken = default) where TEntity : class =>
        await GetQuery(filter, ordering, false).ToListAsync(cancellationToken: cancellationToken).ConfigureAwait(false);

    public virtual ValueTask<TEntity?> FindAsync<TEntity>(params object[] id) where TEntity : class
        => DbContext.Set<TEntity>().FindAsync(id);

    public async ValueTask<TEntity?> FindAsync<TEntity>(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default) where TEntity : class
        => await Get<TEntity>().Where(filter).FirstOrDefaultAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
}