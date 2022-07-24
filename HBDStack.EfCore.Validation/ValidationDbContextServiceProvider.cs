using Microsoft.EntityFrameworkCore;

namespace HBDStack.EfCore.Validation;

/// <summary>
///     This is the serviceProvider used in the validation of data in SaveChanges
///     It allows the developer to access the current DbContext in the IValidateableObject
/// </summary>
internal class ValidationDbContextServiceProvider : IServiceProvider
{
    private readonly DbContext _currContext;

    /// <summary>
    ///     This creates the validation service provider
    /// </summary>
    /// <param name="currContext">The current DbContext in which this validation is happening</param>
    public ValidationDbContextServiceProvider(DbContext currContext) => _currContext = currContext;

    /// <summary>
    ///     This implements the GetService part of the service provider. It only understands the type DbContext
    /// </summary>
    /// <param name="serviceType"></param>
    /// <returns></returns>
    public object? GetService(Type serviceType) => serviceType == typeof(DbContext) ? _currContext : null;
}