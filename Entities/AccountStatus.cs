using System.ComponentModel.DataAnnotations;
using HBDStack.EfCore.Abstractions.Entities;

namespace DataLayer;

public class AccountStatus : Entity<int>
{
    [Required] [MaxLength(100)] public string Name { get; set; }
        
    public AccountStatus()
    {
    }

    public AccountStatus(int id) : base(id)
    {
    }
}