using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Specs
{
    public class UserAccountStartWithDSpec : Spec<User>
    {
        #region Methods

        public override IQueryable<User> Includes(IQueryable<User> query)
        {
            return query.Include(i => i.Account);
        }

        public override Expression<Func<User, bool>> ToExpression()
        {
            return u => u.Account.UserName.StartsWith("D", StringComparison.OrdinalIgnoreCase);
        }

        #endregion Methods
    }
}