using HBDStack.EfCore.Hooks;
using Microsoft.EntityFrameworkCore;

namespace HBDStack.EfCore.DataAuthorization;

internal sealed class DataKeyHook : /*IBeforeSaveHook,*/ IBeforeSaveHookAsync
{
    // public void RunBeforeSave(SnapshotContext context)
    // {
    //     UpdatingDataKey(context);
    // }

    public Task RunBeforeSaveAsync(SnapshotContext context, CancellationToken cancellationToken = default)
    {
        UpdatingDataKey(context);
        return Task.CompletedTask;
    }

    public static void UpdatingDataKey(SnapshotContext context)
    {
        try
        {
            //Turn the auto detect on to capture the changes
            context.DbContext!.ChangeTracker.AutoDetectChangesEnabled = true;

            var dataKeyContext = context.DbContext as IDataKeyDbContext ??
                                 throw new ArgumentException(
                                     $"$The {context.GetType().Name} is must be implementation of IDataKeyContext.");

            if (dataKeyContext.OwnershipKey == Guid.Empty) return;

            var dataKeyEntities = context.SnapshotEntities
                .Where(e => e.OriginalState == EntityState.Added)
                .Select(e => e.Entity)
                .OfType<IDataKeyEntity>();

            foreach (var entity in dataKeyEntities)
                entity.UpdateDataKey(dataKeyContext.OwnershipKey);
        }
        finally
        {
            //Turn the auto detect off back.
            context.DbContext!.ChangeTracker.AutoDetectChangesEnabled = false;
        }
    }
}