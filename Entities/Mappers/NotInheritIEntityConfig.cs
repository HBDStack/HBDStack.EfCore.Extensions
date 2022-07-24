using HBDStack.EfCore.Extensions.Configurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Mappers;

internal class NotInheritIEntityConfig : EntityTypeConfiguration<NotInheritIEntity>
{
    public override void Configure(EntityTypeBuilder<NotInheritIEntity> builder)
    {
        base.Configure(builder);
    }
}