using System.ComponentModel.DataAnnotations;
using HBDStack.EfCore.Abstractions.Attributes;

namespace DataLayer;

[StaticData(nameof(EnumStatus))]
public enum EnumStatus
{
    UnKnow,
    Active,
    InActive
}

[StaticData( "EnumStatusOther")]
public enum EnumStatus1
{
    [Display(Name = "AA", Description = "BB")]
    UnKnow,

    Active,
    InActive
}

[SqlSequence]
public enum Sequences
{
    Order,

    [Sequence(typeof(short), FormatString = "T{DateTime:yyMMdd}{1:00000}", IncrementsBy = 1, Max = short.MaxValue)]
    Invoice,

    [Sequence(typeof(long), IncrementsBy = 1, Max = long.MaxValue)]
    Payment
}