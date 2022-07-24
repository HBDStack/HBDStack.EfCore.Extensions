namespace HBDStack.EfCore.Extensions.Internal;

internal class EntityMappingService
{
    public EntityMappingService(EntityMappingExtension entityMapping) => EntityMapping = entityMapping ?? throw new ArgumentNullException(nameof(entityMapping));

    public EntityMappingExtension EntityMapping { get; }
}