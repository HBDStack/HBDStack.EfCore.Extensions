using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore;

namespace HBDStack.EfCore.Hooks;

/// <summary>
///     The EntitiesSnapshotContext. The Tracking changes of DbContext will be switch off after snapshot the Entities.
///     Call <see cref="Dispose" /> to enable it back.
/// </summary>
public sealed class SnapshotContext : IDisposable
{
    private ImmutableList<SnapshotEntityEntry>? _snapshotEntities;
    public DbContext? DbContext { get; private set; }

    public SnapshotContext(DbContext context) => DbContext = context;

    /// <summary>
    ///     The snapshot of changed entities. Only Entity with status is Modified or Created.
    /// </summary>
    public ImmutableList<SnapshotEntityEntry> SnapshotEntities
    {
        get
        {
            if (_snapshotEntities != null) return _snapshotEntities;

            //DbContext.ChangeTracker.AutoDetectChangesEnabled = false;
            _snapshotEntities = DbContext!.ChangeTracker.Entries()
                .Select(e => new SnapshotEntityEntry(e)).ToImmutableList();

            return _snapshotEntities;
        }
    }

    public void Dispose()
    {
        if (DbContext != null)
            DbContext.ChangeTracker.AutoDetectChangesEnabled = true;
        DbContext = null;
        _snapshotEntities = null;
    }
}