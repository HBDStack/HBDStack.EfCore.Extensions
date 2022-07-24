using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using HBD.EfCore.Repos;
using HBD.EfCore.Repos.Basic;
using HBDStack.EfCore.Abstractions.Pageable;
using HBDStack.EfCore.Abstractions.QueryBuilders;
using Microsoft.EntityFrameworkCore;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global

namespace HBDStack.EfCore.Repos;

public class DtoRepository<TEntity> : IDtoRepository<TEntity> where TEntity : class
{
    private readonly IBasicReadOnlyRepository _repository;
        
    public DtoRepository(IMapper mapper, IBasicReadOnlyRepository repository)
    {
        _repository = repository;
        Mapper = mapper;
    }
        
    protected IMapper Mapper { get; }
        
    public IQueryable<TDto> Get<TDto>(Expression<Func<TEntity, bool>>? filter = null, IQueryBuilder<TDto>? ordering = null) where TDto : class =>
        GetQuery(filter, ordering, false);

    public ValueTask<IPageable<TDto>> PageAsync<TDto>(int pageIndex, int pageSize, IQueryBuilder<TDto> ordering,
        Expression<Func<TEntity, bool>>? filter = null, CancellationToken cancellationToken = default) where TDto : class
    {
        var query = (IOrderedQueryable<TDto>)GetQuery(filter, ordering, false);
        return query.ToPageableAsync(pageIndex, pageSize, cancellationToken: cancellationToken);
    }

    public ValueTask<IPageable<TDto>> PageWithoutGlobalFiltersAsync<TDto>(int pageIndex, int pageSize,
        IQueryBuilder<TDto> ordering, Expression<Func<TEntity, bool>>? filter = null, CancellationToken cancellationToken = default) where TDto : class
    {
        var query = (IOrderedQueryable<TDto>)GetQuery(filter, ordering, true);
        return query.ToPageableAsync(pageIndex, pageSize, cancellationToken: cancellationToken);
    }

    public async ValueTask<TDto?> FindAsync<TDto>(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default) where TDto : class 
        => await GetQuery<TDto>(filter, null, false).FirstOrDefaultAsync(cancellationToken: cancellationToken).ConfigureAwait(false);

    /// <summary>
    ///     Get TEntity query
    /// </summary>
    /// <param name="ignoreFilters"></param>
    /// <returns></returns>
    protected virtual IQueryable<TEntity> GetEntityQuery(bool ignoreFilters)
    {
        var query = ignoreFilters ? _repository.Get<TEntity>().IgnoreQueryFilters() : _repository.Get<TEntity>();
        return query.TrackingChanges(false);
    }
        
    protected virtual IQueryable<TDto> GetQuery<TDto>(Expression<Func<TEntity, bool>>? filter, IQueryBuilder<TDto>? ordering, bool ignoreFilters) where TDto : class
    {
        var query = GetEntityQuery(ignoreFilters);
        if (filter != null)
            query = query.Where(filter);

        return ordering != null 
            ? ordering.Build(query.ProjectTo<TDto>(Mapper.ConfigurationProvider)) 
            : query.ProjectTo<TDto>(Mapper.ConfigurationProvider);
    }
}