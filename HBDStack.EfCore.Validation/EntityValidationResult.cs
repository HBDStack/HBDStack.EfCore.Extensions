using System.ComponentModel.DataAnnotations;

namespace HBDStack.EfCore.Validation;

public sealed class EntityValidationResult
{
    public object Entity { get; }
    public IEnumerable<ValidationResult> ValidationResults { get; }

    public EntityValidationResult(object entity, IEnumerable<ValidationResult>validationResults)
    {
        Entity = entity ?? throw new ArgumentNullException(nameof(entity));
        ValidationResults = validationResults ?? throw new ArgumentNullException(nameof(validationResults));
    }
}