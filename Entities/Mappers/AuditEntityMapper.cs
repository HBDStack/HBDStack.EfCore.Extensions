using HBDStack.EfCore.Extensions.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Mappers;

internal class AuditEntityMapper<T> : EntityTypeConfiguration<T> where T : BaseEntity
{
    #region Properties

    public static bool Called { get; private set; }

    #endregion Properties

    #region Methods

    public override void Configure(EntityTypeBuilder<T> builder)
    {
        Called = true;

        base.Configure(builder);
        builder.HasIndex(c => c.Id).IsUnique();
        builder.Property(c => c.Id).UseIdentityColumn();
    }

    #endregion Methods
}