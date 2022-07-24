using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Specs
{
    public class UserIncludeAccountSpec : Spec<User>
    {
        #region Methods

        public override IQueryable<User> Includes(IQueryable<User> query)
        {
            return query.Include(u => u.Account);
        }

        public override Expression<Func<User, bool>> ToExpression()
        {
            return All.ToExpression();
        }

        #endregion Methods
    }
}