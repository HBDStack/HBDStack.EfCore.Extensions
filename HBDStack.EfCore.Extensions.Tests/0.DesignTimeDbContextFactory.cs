using DataLayer;
using DataLayer.Mappers;
using HBDStack.EfCore.Extensions.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace HBDStack.EfCore.Extensions.Tests;

internal class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<MyDbContext>
{
    #region Public Methods

    public MyDbContext CreateDbContext(string[] args)
    {
        return new MyDbContext(new DbContextOptionsBuilder()
            .UseSqlServer(WithSqlDbTests.ConnectionString)
            .UseAutoConfigModel(op => op.ScanFrom(typeof(MyDbContext).Assembly)
                .WithDefaultMappersType(typeof(AuditEntityMapper<>), typeof(BaseEntityMapper<>),
                    typeof(EntityTypeConfiguration<>)))
            .Options);
    }

    #endregion Public Methods
}