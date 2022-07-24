namespace HBDStack.EfCore.Abstractions.QueryBuilders;

public interface IQueryBuilder<T> where T : class
{
    #region Methods

    IQueryable<T> Build(IQueryable<T> query);

    #endregion Methods
}