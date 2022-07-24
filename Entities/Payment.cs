using System;
using System.ComponentModel.DataAnnotations.Schema;
using HBDStack.EfCore.Abstractions.Entities;

namespace DataLayer;

public class Payment : Entity<string>
{
    public Payment() : base(Guid.NewGuid().ToString())
    {
        PaidOn = DateTime.Now;
    }

    public DateTime PaidOn { get; }

    [Column(TypeName = "Money")] public decimal Amount { get; set; }
}