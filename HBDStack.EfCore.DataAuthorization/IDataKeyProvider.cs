namespace HBDStack.EfCore.DataAuthorization;

public interface IDataKeyProvider
{
    #region Methods

    /// <summary>
    ///     The collection of data key for Impersonate. This will be use for Global query filter.
    /// </summary>
    IEnumerable<Guid> GetImpersonateKeys();

    /// <summary>
    ///     The collection of data key for Ownership Key. This key will be updated to entity DataKey when creating.
    ///     Return an empty key if not available.
    /// </summary>
    Guid GetOwnershipKey();

    #endregion Methods
}