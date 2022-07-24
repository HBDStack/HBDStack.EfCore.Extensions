namespace HBDStack.EfCore.Hooks;

/// <summary>
///     The interface Hook allows to handle the Before Save actions of DbContext
/// </summary>
// public interface IBeforeSaveHook
// {
//     void RunBeforeSave(SnapshotContext context);
// }

/// <summary>
///     The interface Hook allows to handle the After Save actions of DbContext
/// </summary>
// public interface IAfterSaveHook
// {
//     void RunAfterSave(SnapshotContext context);
// }

/// <summary>
///     The interface Hook allows to handle the Before Save Async actions of DbContext
/// </summary>
public interface IBeforeSaveHookAsync
{
    Task RunBeforeSaveAsync(SnapshotContext context, CancellationToken cancellationToken = default);
}

/// <summary>
///     The interface Hook allows to handle the After Save Async actions of DbContext
/// </summary>
public interface IAfterSaveHookAsync
{
    Task RunAfterSaveAsync(SnapshotContext context, CancellationToken cancellationToken = default);
}

/// <summary>
///     The interface Hook allows to handle the Before Save and After Save actions of DbContext
/// </summary>
// public interface IHook : IBeforeSaveHook, IAfterSaveHook
// {
// }

/// <summary>
///     The interface HookAsync allows to handle the Before Save and After Save async actions of DbContext
/// </summary>
public interface IHookAsync : IBeforeSaveHookAsync, IAfterSaveHookAsync
{
}