using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Specs
{
    public class UserIdGreaterThan10Spec : Spec<User>
    {
        #region Methods

        public override IQueryable<User> Includes(IQueryable<User> query)
        {
            return query.Include(u => u.Addresses);
        }

        public override Expression<Func<User, bool>> ToExpression()
        {
            return u => u.Id > 10;
        }

        #endregion Methods
    }
}