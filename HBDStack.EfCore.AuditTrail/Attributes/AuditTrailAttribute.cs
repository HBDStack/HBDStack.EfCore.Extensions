using HBDStack.EfCore.AuditTrail.Entities;

namespace HBDStack.EfCore.AuditTrail.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public sealed class AuditTrailAttribute : Attribute
{
    /// <summary>
    /// The properties will be ignored from the auditing values.
    /// </summary>
    public string[] IgnoredProperties { get; set; }
    internal Type AuditTrailItemType { get; private set; }

    /// <summary>
    /// Customize this allows to write the audit trail item to a different tables.
    /// </summary>
    /// <param name="auditTrailItemType">The type must be inherited from <see cref="AuditItem"/></param>
    public AuditTrailAttribute(Type? auditTrailItemType = null)
    {
        IgnoredProperties = Enumerable.Empty<string>().ToArray();
        AuditTrailItemType = auditTrailItemType ?? AuditTrailModelBuilderRegister.DefaultAuditItemType;
        
        if(!AuditTrailItemType.IsAssignableTo(typeof(AuditItem)))
            throw new ArgumentException($"{nameof(auditTrailItemType)} must be inherited from {nameof(AuditItem)}");
    }
}