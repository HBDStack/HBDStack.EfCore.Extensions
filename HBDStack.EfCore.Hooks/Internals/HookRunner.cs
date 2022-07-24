using Microsoft.EntityFrameworkCore.Diagnostics;

namespace HBDStack.EfCore.Hooks.Internals;

public enum RunningTypes
{
    BeforeSave,
    AfterSave
}

internal sealed class HookRunner : ISaveChangesInterceptor
{
    private readonly IEnumerable<IAfterSaveHookAsync> _afterSaveHookAsync;
    //private readonly IEnumerable<IAfterSaveHook> _afterSaveHooks;
    private readonly IEnumerable<IBeforeSaveHookAsync> _beforeSaveHookAsync;
    //private readonly IEnumerable<IBeforeSaveHook> _beforeSaveHooks;

    private SnapshotContext? _dataProvider;

    public HookRunner(
        //IEnumerable<IBeforeSaveHook> beforeSaveHooks,
        IEnumerable<IBeforeSaveHookAsync> beforeSaveHookAsync,
        //IEnumerable<IAfterSaveHook> afterSaveHooks,
        IEnumerable<IAfterSaveHookAsync> afterSaveHookAsync)
    {
        //_beforeSaveHooks = beforeSaveHooks;
        _beforeSaveHookAsync = beforeSaveHookAsync;
        //_afterSaveHooks = afterSaveHooks;
        _afterSaveHookAsync = afterSaveHookAsync;
    }

    #region RunHooks

    // private void RunHooks(RunningTypes type, SnapshotContext context)
    // {
    //     if(context==null)throw new ArgumentNullException(nameof(context));
    //     
    //     //Run Hooks
    //     if (type == RunningTypes.BeforeSave)
    //         foreach (var h in _beforeSaveHooks.Where(h=>!context.DbContext.IsHookDisabled(h)))
    //             h.RunBeforeSave(context);
    //     else
    //         foreach (var h in _afterSaveHooks.Where(h=>!context.DbContext.IsHookDisabled(h)))
    //             h.RunAfterSave(context);
    // }

    private async Task RunHooksAsync(RunningTypes type, SnapshotContext context, CancellationToken cancellationToken = default)
    {
        if(context==null)throw new ArgumentNullException(nameof(context));
 
        //Run Hooks Async
        if (type == RunningTypes.BeforeSave)
            foreach (var h in _beforeSaveHookAsync.Where(h=>!context.DbContext!.IsHookDisabled(h)))
                await h.RunBeforeSaveAsync(context, cancellationToken).ConfigureAwait(false);
        else
            foreach (var h in _afterSaveHookAsync.Where(h=>!context.DbContext!.IsHookDisabled(h)))
                await h.RunAfterSaveAsync(context, cancellationToken).ConfigureAwait(false);
    }

    #endregion

    #region Save Changes

    // public InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    // {
    //     _dataProvider = new SnapshotContext(eventData.Context);
    //     //Run Before Save
    //     RunHooks(RunningTypes.BeforeSave, _dataProvider);
    //
    //     return result;
    // }

    // public int SavedChanges(SaveChangesCompletedEventData eventData, int result)
    // {
    //     try
    //     {
    //         //Run After Save.
    //         RunHooks(RunningTypes.AfterSave, _dataProvider);
    //     }
    //     finally
    //     {
    //         _dataProvider?.Dispose();
    //         _dataProvider = null;
    //     }
    //
    //     return result;
    // }
    
    public InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        => throw new NotSupportedException($"Please use {nameof(SavingChangesAsync)} version");
    
    public int SavedChanges(SaveChangesCompletedEventData eventData, int result) 
        => throw new NotSupportedException($"Please use {nameof(SavingChangesAsync)} version");

    #endregion

    #region Save Changes Async

    public async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        _dataProvider = new SnapshotContext(eventData.Context!);

        //Run Before Save
        await RunHooksAsync(RunningTypes.BeforeSave, _dataProvider, cancellationToken).ConfigureAwait(false);

        return result;
    }

    public async ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result,
        CancellationToken cancellationToken = default)
    {
        try
        {
            //Run After Events and ignore the result even failed.
            await RunHooksAsync(RunningTypes.AfterSave, _dataProvider!, cancellationToken).ConfigureAwait(false);
        }
        finally
        {
            _dataProvider?.Dispose();
            _dataProvider = null;
        }

        return result;
    }

    #endregion

    #region Saved Failed

    public void SaveChangesFailed(DbContextErrorEventData eventData)
    {
    }

    public Task SaveChangesFailedAsync(DbContextErrorEventData eventData, CancellationToken cancellationToken = default) => Task.CompletedTask;

    #endregion
}