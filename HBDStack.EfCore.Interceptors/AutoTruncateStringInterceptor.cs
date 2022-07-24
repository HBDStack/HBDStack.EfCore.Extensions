using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace HBDStack.EfCore.Interceptors;

public class AutoTruncateStringInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        TruncateString(eventData);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = new CancellationToken())
    {
        TruncateString(eventData);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void TruncateString(DbContextEventData eventData)
    {
        if (eventData.Context == null) return;
        var entities = eventData.Context.ChangeTracker.Entries().Where(e => e.State is EntityState.Added or EntityState.Modified);
        var maxLengthMetadata = eventData.Context.GetMaxLengthMetadata();

        foreach (var entity in entities)
        {
            var props = entity.CurrentValues.Properties.Where(p => p.ClrType == typeof(string));
            foreach (var prop in props)
            {
                if (entity.CurrentValues[prop.Name] == null) continue;
                var stringValue = entity.CurrentValues[prop.Name]!.ToString();
                if (maxLengthMetadata.ContainsKey(prop))
                {
                    var maxLength = maxLengthMetadata[prop];
                    stringValue = TruncateString(stringValue, maxLength);
                }

                entity.CurrentValues[prop.Name] = stringValue;
            }
        }
    }

    private static string? TruncateString(string? value, int maxLength)
    {
        if (string.IsNullOrEmpty(value)) return value;
        return value.Length <= maxLength ? value : value.Substring(0, maxLength);
    }
}