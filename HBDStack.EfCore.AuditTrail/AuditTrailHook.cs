using System.Reflection;
using HBDStack.EfCore.Abstractions.Entities;
using HBDStack.EfCore.Hooks;
using HBDStack.EfCore.AuditTrail.Attributes;
using HBDStack.EfCore.AuditTrail.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HBDStack.EfCore.AuditTrail;

public sealed class AuditTrailHook : IHookAsync
{
    private readonly List<dynamic> _auditTrialItems = new();

    private readonly ILogger<AuditTrailHook> _logger;

    public AuditTrailHook(ILogger<AuditTrailHook> logger) => _logger = logger;

    public Task RunBeforeSaveAsync(SnapshotContext context, CancellationToken cancellationToken = default)
    {
        foreach (var entry in context.SnapshotEntities)
        {
            //Only IAuditEntity will be compatible with AuditTrail
            if (entry.Entity is AuditItem or not IAuditProperties ||
                entry.OriginalState is EntityState.Detached or EntityState.Unchanged) continue;

            //Only apply audit trail for entities marked with attribute.
            var att = entry.Entry.Metadata.ClrType.GetCustomAttribute<AuditTrailAttribute>();
            if (att is null) continue;
            
            _auditTrialItems.Add(new AuditEntry(entry.Entry, att));
        }

        return Task.CompletedTask;
    }

    public async Task RunAfterSaveAsync(SnapshotContext context, CancellationToken cancellationToken = default)
    {
        //Disable this hook to prevent infinite loop.
        using var disableOtherHooks = context.DbContext!.DisposableHooks(this);

        try
        {
            //Collect all audit trail items.
            foreach (var entry in _auditTrialItems)
                await context.DbContext!.AddAsync(entry!.ToAudit(), cancellationToken).ConfigureAwait(false);

            //Save to database.
            await context.DbContext!.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while saving audit trail");
        }

        //Clear all audit trail items.
        _auditTrialItems.Clear();
    }
}