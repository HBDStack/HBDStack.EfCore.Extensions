using System.Threading.Tasks;
using DataLayer;
using HBDStack.EfCore.Abstractions.QueryBuilders;
using HBDStack.TestHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HBDStack.EfCore.Extensions.Tests;

[TestClass]
public class DynamicOrderTests
{
    [TestMethod]
    public async Task MultiDynamicOrders()
    {
        var result = await UnitTestSetup.Db.Set<User>()
            .AsQueryable()
            .OrderByDynamics(new DynamicOrderInfo("FirstName",OrderingDirection.Asc),new DynamicOrderInfo("Account.UserName",OrderingDirection.Asc))
            .ToListAsync();
    }
}