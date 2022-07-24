using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HBDStack.EfCore.Abstractions.Entities;

public interface IConcurrencyEntity
{
    [Column(Order = 1000)]
    [Timestamp, ConcurrencyCheck]
    byte[]? RowVersion { get; }
    
    void SetRowVersion(byte[] rowVersion);
}