namespace HBDStack.EfCore.Validation;

public class EntityValidationException: Exception
{
    public EntityValidationException(IEnumerable<EntityValidationResult>results,string message = "There are some entity validation failed.") : base(message)
    {
    }
}
