using System;
using System.Collections.Generic;
using HBDStack.EfCore.DataAuthorization;

namespace DDD4Tests;

public class TestDataKeyProvider : IDataKeyProvider
{
    public IEnumerable<Guid> GetImpersonateKeys() => new[] {GetOwnershipKey()};

    public Guid GetOwnershipKey() => new("bc2c7648-6e0e-41f9-adff-b344302fdc8d");
}