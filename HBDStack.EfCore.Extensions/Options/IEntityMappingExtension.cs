using System.Reflection;

namespace HBDStack.EfCore.Extensions.Options;

public interface IEntityMappingExtension
{
    /// <summary>
    ///     The Assemblies will be scan
    /// </summary>
    /// <param name="entityAssemblies"></param>
    /// <returns></returns>
    RegistrationInfo ScanFrom(params Assembly[] entityAssemblies);
}