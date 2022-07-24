using System.Reflection;
using HBDStack.EfCore.Extensions.Options;
using HBDStack.EfCore.Extensions.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

// ReSharper disable UnusedMember.Global

namespace HBDStack.EfCore.Extensions.Internal;

internal class EntityMappingExtension : IDbContextOptionsExtension, IEntityMappingExtension
{
    //private bool _disableGlobalQueryFilterScan;
    private DbContextOptionsExtensionInfo? _info;
    private Action<IServiceCollection>? _extraServiceProvider;
        
    public DbContextOptionsExtensionInfo Info => _info ??= new EntityMappingExtensionInfo(this);
    internal ICollection<RegistrationInfo> Registrations { get; } = new List<RegistrationInfo>();

    public void ApplyServices(IServiceCollection services)
    {
        //Add custom services
        _extraServiceProvider?.Invoke(services);

        //Add EntityMappingService, it should be Singleton
        services.AddSingleton(new EntityMappingService(this));

        //Replace the IModelCustomizer with ExtraModelCustomizer. This only available for Relational Db.
        var originalDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(IModelCustomizer));

        if (originalDescriptor == null)
        {
            //it should be Singleton
            services.AddSingleton<ModelCustomizer, RelationalModelCustomizer>();
            services.Add(new ServiceDescriptor(typeof(IModelCustomizer), typeof(ExtraModelCustomizer), ServiceLifetime.Singleton));
        }
        else
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            services.Add(new ServiceDescriptor(typeof(ModelCustomizer), originalDescriptor.ImplementationType!,
                originalDescriptor.Lifetime));
            services.Replace(new ServiceDescriptor(typeof(IModelCustomizer), typeof(ExtraModelCustomizer),
                originalDescriptor.Lifetime));
        }

        //Register Global Query Filter from registration, it should be Singleton
        foreach (var g in SetupEfCore.GlobalQueryFilters)
            services.Add(
                new ServiceDescriptor(typeof(IGlobalModelBuilderRegister), g, ServiceLifetime.Singleton));
    }

    /// <summary>
    ///     Register extra service to internal IServiceCollection of EfCore.
    /// </summary>
    /// <param name="provider"></param>
    public void AddExtraServices(Action<IServiceCollection> provider) => _extraServiceProvider = provider;

    /// <inheritdoc />
    /// <summary>
    ///     The Assemblies will be scan.
    ///     The IGlobalQueryFilterRegister also will be scanned from these assemblies and load into internal ServiceProvider.
    ///     If you have any IGlobalQueryFilterRegister implementation. Just provider the assemblies here and no need to add
    ///     them to any other DI.
    /// </summary>
    /// <param name="assembliesToScans"></param>
    /// <returns></returns>
    public RegistrationInfo ScanFrom(params Assembly[] assembliesToScans)
    {
        if (!assembliesToScans.Any())
            assembliesToScans = new[] {Assembly.GetCallingAssembly()};

        var register = new RegistrationInfo(assembliesToScans);
        Registrations.Add(register);
        return register;
    }

    public void Validate(IDbContextOptions options)
    {
        foreach (var info in Registrations)
            info.Validate();
    }
}