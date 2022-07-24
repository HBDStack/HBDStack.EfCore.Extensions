using System.Globalization;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace HBDStack.EfCore.Extensions.Internal;

internal sealed class EntityMappingExtensionInfo : DbContextOptionsExtensionInfo
{
    #region Constructors

    public EntityMappingExtensionInfo(EntityMappingExtension extension) : base(extension)
    {
    }

    #endregion Constructors

    #region Properties

    public override bool IsDatabaseProvider => false;

    public override string LogFragment => $"using {nameof(EntityMappingExtension)}";

    #endregion Properties

    #region Methods

#if  NET6_0
    public override int GetServiceProviderHashCode() => (nameof(EntityMappingExtension).GetHashCode(StringComparison.CurrentCulture) * 3) ^ GetHashCode();

    public override bool ShouldUseSameServiceProvider(DbContextOptionsExtensionInfo other) => other is EntityMappingExtensionInfo;
#else
         public override long GetServiceProviderHashCode() => (nameof(EntityMappingExtension).GetHashCode(StringComparison.CurrentCulture) * 3) ^ GetHashCode();
#endif
       
    public override void PopulateDebugInfo(IDictionary<string, string> debugInfo)
    {
        if (debugInfo is null)
            throw new ArgumentNullException(nameof(debugInfo));

        debugInfo["Core:" + nameof(EntityMappingExtension)] =
            GetServiceProviderHashCode().ToString(CultureInfo.InvariantCulture);
    }

    #endregion Methods
}