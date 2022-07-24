using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using DataLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HBDStack.EfCore.Extensions.Tests;

[TestClass]
public class BenchmarkTests
{
    #region Public Methods

    [Benchmark]
    public async Task TestCreateDb_CustomMapper()
    {
        await using var db = new MyDbContext(new DbContextOptionsBuilder()
            .UseSqliteMemory()
            //No Assembly provided it will scan the MyDbContext assembly.
            .UseAutoConfigModel(op => op.ScanFrom(typeof(MyDbContext).Assembly))
            .Options);
        await db.Database.EnsureCreatedAsync().ConfigureAwait(false);
    }

    [TestMethod]
    public void TestRegister()
    {
        var summary = BenchmarkRunner.Run<BenchmarkTests>();
    }


    // [TestMethod]
    // public void TestSpecs()
    // {
    //     var summary = BenchmarkRunner.Run<SpecTests>();
    // }

    #endregion Public Methods
}