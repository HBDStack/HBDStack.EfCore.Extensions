using System.Diagnostics;
using System.Reflection;
using HBDStack.EfCore.Extensions.Services;
using Microsoft.EntityFrameworkCore;

// ReSharper disable UnusedType.Global

namespace HBDStack.EfCore.DataAuthorization;

/// <summary>
///     This
/// </summary>
public class DataKeyAuthQueryRegister : IGlobalModelBuilderRegister
{
    private static readonly MethodInfo Method = typeof(DataKeyAuthQueryRegister)
        .GetMethod(nameof(ApplyQueryFilter), BindingFlags.Static | BindingFlags.NonPublic)!;

    public void Apply(ModelBuilder? modelBuilder, DbContext context)
    {
        if (modelBuilder == null)
        {
            Trace.WriteLine($"{nameof(DataKeyAuthQueryRegister)}-Ignored: ModelBuilder is null.");
            return;
        }

        if (context is not IDataKeyDbContext keyContext)
        {
            Trace.WriteLine(
                $"{nameof(DataKeyAuthQueryRegister)}-Ignored: DbContext is not an implementation of IDataKeyContext.");
            return;
        }

        //Apply Global query filter to all IDataKeyEntity
        foreach (var t in modelBuilder.Model.GetEntityTypes()
                     .Where(t => typeof(IDataKeyEntity).IsAssignableFrom(t.ClrType)))
        {
            //Ignore the Delivered from Inheritance
#if NET6_0
                if (t.GetDiscriminatorValue() != null)
                    continue;
#else
            if (t.GetDiscriminatorProperty() != null
                && t.GetDiscriminatorValue() != null)
                continue;
#endif
            var md = Method.MakeGenericMethod(t.ClrType);
            md.Invoke(null, new object[] { modelBuilder, keyContext });
        }
    }

    private static void ApplyQueryFilter<TEntity>(ModelBuilder modelBuilder, IDataKeyDbContext context)
        where TEntity : class, IDataKeyEntity
    {
        modelBuilder.Entity<TEntity>()
            .HasQueryFilter(x => x.DataKey == null || context.ImpersonateKeys.Contains(x.DataKey.Value));
    }
}