using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace HBDStack.EfCore.Validation;

public interface IEntityValidator
{
    Task<ICollection<ValidationResult>> ValidateAsync(object entity, DbContext dbContext,
        CancellationToken token = default);
}

public class DefaultEntityValidator : IEntityValidator
{
    public virtual Task<ICollection<ValidationResult>> ValidateAsync(object entity, DbContext dbContext,
        CancellationToken token = default)
        => Task.FromResult(Validate(entity, dbContext));

    private static ICollection<ValidationResult> Validate(object entity, DbContext dbContext)
    {
        var valProvider = new ValidationDbContextServiceProvider(dbContext);
        var valContext = new ValidationContext(entity, valProvider, null);
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(entity, valContext, results, true);
        return results;
    }
}