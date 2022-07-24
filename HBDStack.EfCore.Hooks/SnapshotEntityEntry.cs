using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace HBDStack.EfCore.Hooks;

public sealed class SnapshotEntityEntry
{
    public EntityEntry Entry { get; }
    
    /// <summary>
    /// The original stage before saved changes.
    /// </summary>
    public EntityState OriginalState { get; }

    public object Entity => Entry.Entity;

    public SnapshotEntityEntry(EntityEntry entry)
    {
        Entry = entry;
        OriginalState = entry.State;
    }
}