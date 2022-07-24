﻿using Microsoft.EntityFrameworkCore;

namespace HBDStack.EfCore.Extensions.Services;

/// <summary>
///     This will be scans from the Assemblies when registering the services.
/// Use this to apply the global filter for entity or add custom entities into DbContext.
/// </summary>
public interface IGlobalModelBuilderRegister
{
    void Apply(ModelBuilder modelBuilder, DbContext context);
}