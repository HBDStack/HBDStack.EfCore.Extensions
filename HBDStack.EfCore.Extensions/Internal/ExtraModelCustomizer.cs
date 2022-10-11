using HBDStack.EfCore.Extensions.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace HBDStack.EfCore.Extensions.Internal;

internal class ExtraModelCustomizer : IModelCustomizer
{
    #region Constructors

    public ExtraModelCustomizer(ModelCustomizer original,
        IEnumerable<IGlobalModelBuilderRegister> globalQueryFilters)
    {
        _original = original;
        _globalQueryFilters = globalQueryFilters;
    }

    #endregion Constructors

    #region Fields

    private readonly IModelCustomizer _original;
    private readonly IEnumerable<IGlobalModelBuilderRegister> _globalQueryFilters;

    #endregion Fields

    #region Methods

    public void Customize(ModelBuilder modelBuilder, DbContext context)
    {
        ConfigModelCreating(context, modelBuilder);
        _original?.Customize(modelBuilder, context);
    }

    private void ConfigModelCreating(DbContext dbContext, ModelBuilder modelBuilder)
    {
        if (dbContext == null) throw new ArgumentNullException(nameof(dbContext));
        if (modelBuilder == null) throw new ArgumentNullException(nameof(modelBuilder));

        var options = dbContext.GetService<EntityMappingService>()?.EntityMapping;
        if (options == null) return;

        if (options.Registrations.Count <= 0)
            options.ScanFrom(dbContext.GetType().Assembly);

        //Register Entities
        modelBuilder.RegisterEntityMappingFrom(options.Registrations);

        //Register StaticData Of
        modelBuilder.RegisterStaticDataFrom(options.Registrations);

        //Register Data Seeding
        modelBuilder.RegisterDataSeedingFrom(options.Registrations);

        //Register Sequence
        if (dbContext.Database.IsSequenceSupported())
            modelBuilder.RegisterSequencesFrom(options.Registrations);

        //Config DataKey
        _globalQueryFilters.ForEach(g => g.Apply(modelBuilder, dbContext));
    }

    #endregion Methods
}