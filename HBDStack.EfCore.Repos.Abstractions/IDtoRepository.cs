using System.Linq.Expressions;
using HBDStack.EfCore.Abstractions.Pageable;
using HBDStack.EfCore.Abstractions.QueryBuilders;

namespace HBDStack.EfCore.Repos.Abstractions;

public interface IDtoRepository<TEntity> where TEntity : class
{
    #region Methods

    /// <summary>
    ///     Read List DTO with entity filtering
    /// </summary>
    public IQueryable<TDto> Get<TDto>(Expression<Func<TEntity, bool>>? filter = null, IQueryBuilder<TDto>? ordering = null) where TDto : class;

    public ValueTask<IPageable<TDto>> PageAsync<TDto>(int pageIndex, int pageSize, IQueryBuilder<TDto> ordering,
        Expression<Func<TEntity, bool>>? filter = null, CancellationToken cancellationToken = default) where TDto : class;

    public ValueTask<IPageable<TDto>> PageWithoutGlobalFiltersAsync<TDto>(int pageIndex, int pageSize,
        IQueryBuilder<TDto> ordering, Expression<Func<TEntity, bool>>? filter = null, CancellationToken cancellationToken = default) where TDto : class;

    public ValueTask<TDto?> FindAsync<TDto>(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default) where TDto : class;

    #endregion Methods
}