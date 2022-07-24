namespace HBDStack.EfCore.Abstractions.Attributes;

/// <summary>
///     Create Sql Sequence in Db according to Enum fields
///     only support type int, short, byte, and long
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class SequenceAttribute : Attribute
{
    private static readonly ICollection<Type> SupportedTypes = new[] {typeof(byte), typeof(short), typeof(int), typeof(long)};
    
    public SequenceAttribute(Type? type = null)
    {
        Type = type ?? typeof(int);
        if (!SupportedTypes.Contains(Type))
            throw new NotSupportedException(Type.Name);
    }
    
    public bool Cyclic { get; set; } = true;

    /// <summary>
    /// </summary>
    public string? FormatString { get; set; }

    public int IncrementsBy { get; set; } = -1;

    public long Max { get; set; } = -1;

    public long Min { get; set; } = -1;

    public long StartAt { get; set; } = -1;

    public Type Type { get; }
}

[AttributeUsage(AttributeTargets.Enum)]
public class SqlSequenceAttribute : Attribute
{
    public string Schema { get; set; } = "seq";
}