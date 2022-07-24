using System.Collections.ObjectModel;

namespace HBDStack.EfCore.Abstractions.Pageable;

public class Pageable<TEntity> : IPageable<TEntity>
{
    #region Constructors

    public Pageable(int pageIndex, int pageSize, int totalItems, IList<TEntity> items)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
        TotalItems = totalItems;
        Items = new ReadOnlyCollection<TEntity>(items);
    }

    #endregion Constructors

    #region Properties

    public IReadOnlyCollection<TEntity> Items { get; }

    public int PageIndex { get; }

    public int PageSize { get; }

    public int TotalItems { get; }

    public int TotalPage => TotalItems / PageSize + (TotalItems % PageSize > 0 ? 1 : 0);

    #endregion Properties
}