﻿using System.ComponentModel.DataAnnotations;
using HBDStack.EfCore.Abstractions.Entities;

namespace DataLayer;

public class Account : AuditEntity<int>
{
    [Required] public string Password { get; set; }
    [Required] public string UserName { get; set; }
}