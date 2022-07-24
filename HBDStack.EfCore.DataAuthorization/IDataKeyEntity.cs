namespace HBDStack.EfCore.DataAuthorization;

public interface IDataKeyEntity
{
    #region Properties

    /// <summary>
    ///     Multi tenant DataKey and allow null. Null means there is no ownership or sharing entity for all tenants
    /// </summary>
    Guid? DataKey { get; }

    #endregion Properties

    #region Methods

    /// <summary>
    ///     Update the DataKey this only call when creating the object. Or change the ownership of entity
    /// </summary>
    /// <param name="dataKey"></param>
    void UpdateDataKey(Guid dataKey);

    #endregion Methods
}