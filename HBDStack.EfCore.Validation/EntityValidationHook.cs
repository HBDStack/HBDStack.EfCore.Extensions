using HBDStack.EfCore.Hooks;
using Microsoft.EntityFrameworkCore;

namespace HBDStack.EfCore.Validation;

internal sealed class EntityValidationHook : IBeforeSaveHookAsync
{
    private readonly IEntityValidator _validator;
    public EntityValidationHook(IEntityValidator validator) => _validator = validator;

    public async Task RunBeforeSaveAsync(SnapshotContext context, CancellationToken cancellationToken = default)
    {
        var entities =
            context.SnapshotEntities.Where(e => e.OriginalState is EntityState.Added or EntityState.Modified);

        var errors = new List<EntityValidationResult>();

        foreach (var entity in entities)
        {
            var rs = await _validator.ValidateAsync(entity.Entity, context.DbContext!, cancellationToken)
                .ConfigureAwait(false);
            if (rs.Any())
                errors.Add(new EntityValidationResult(entity.Entity, rs));
        }

        if (errors.Any())
            throw new EntityValidationException(errors);
    }
}