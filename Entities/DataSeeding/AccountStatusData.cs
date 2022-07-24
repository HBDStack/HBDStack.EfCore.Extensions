using System.Collections.Generic;
using HBDStack.EfCore.Extensions.Configurations;

namespace DataLayer.DataSeeding;

public class AccountStatusData : IDataSeedingConfiguration<AccountStatus>
{
    #region Properties

    public ICollection<AccountStatus> Data => new[]
    {
        new AccountStatus(1)
        {
            Name = "Duy"
        },
        new AccountStatus(2)
        {
            Name = "Hoang"
        }
    };

    #endregion Properties
}