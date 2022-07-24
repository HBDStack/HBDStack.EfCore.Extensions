using HBDStack.EfCore.Abstractions.Entities;

namespace HBDStack.EfCore.Extensions.Internal;

internal sealed class EnumTables<T> : IEntity<int>
{
    #region Properties

    // ReSharper disable once UnusedAutoPropertyAccessor.Local
    public int Id { get; private set; }

    #endregion Properties
}