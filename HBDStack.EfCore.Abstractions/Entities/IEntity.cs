using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HBDStack.EfCore.Abstractions.Entities;

public interface IEntity<out TKey>
{
    [Key]
    [Column(Order = 1)]
    TKey Id { get; }
}

