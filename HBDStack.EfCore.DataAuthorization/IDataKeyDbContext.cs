namespace HBDStack.EfCore.DataAuthorization;

public interface IDataKeyDbContext
{
    #region Properties

    /// <summary>
    ///     The collection of data key for Impersonate. This will be use for Global query filter.
    /// </summary>
    IEnumerable<Guid> ImpersonateKeys { get; }

    /// <summary>
    ///     The collection of data key for Ownership Key. This key will be updated to entity DataKey when creating.
    /// </summary>
    Guid OwnershipKey { get; }

    #endregion Properties
}