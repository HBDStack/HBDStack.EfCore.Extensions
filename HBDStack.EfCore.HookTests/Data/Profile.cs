using System;
using System.ComponentModel.DataAnnotations;
using HBDStack.EfCore.Abstractions.Entities;

namespace HBDStack.EfCore.HookTests.Data;

public class Profile : IEntity<Guid>
{
    public Profile() => Id = Guid.Empty;

    [Required]
    public string Name { get; set; }

    public Guid Id { get; set; }
}