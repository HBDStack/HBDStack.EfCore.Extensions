using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using DataLayer;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HBDStack.EfCore.Extensions.Tests;

[TestClass]
public class WithSqlDbTests
{
    private static MyDbContext _db;
    
    public static string ConnectionString =>
        RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=UnitTestDb;Integrated Security=True;Connect Timeout=30;"
            : "Data Source=192.168.1.95;Initial Catalog=UnitTestDb;User Id=sa;Password=Pass@word1;";

    [ClassCleanup]
    public static void CleanUp()
    {
        _db.Database.EnsureDeleted();
        _db.Dispose();
    }

    [ClassInitialize]
    public static void Setup(TestContext _)
    {
        _db = new MyDbContext(new DbContextOptionsBuilder()
            .UseSqlServer(ConnectionString)
            .UseAutoConfigModel(op => op.ScanFrom(typeof(MyDbContext).Assembly))
            .Options);

        _db.Database.EnsureCreated();
    }

    [TestMethod]
    public async Task SequenceValueTestAsync()
    {
        var val1 = await _db.NextSeqValue<Sequences, short>(Sequences.Invoice);
        val1.Should().BeGreaterThan(0);

        var val2 = await _db.NextSeqValue<Sequences, int>(Sequences.Order);
        val2.Should().BeGreaterThan(0);
    }

    [TestMethod]
    public async Task SequenceValueWithFormatTestAsync()
    {
        var val1 = await _db.NextSeqValueWithFormat(Sequences.Invoice);
        val1.Should().Contain(string.Format("T{0:yyMMdd}0000", DateTime.Now));
    }

    [TestMethod]
    public async Task Test_Create_WithSqlDb_Async()
    {
        //Create User with Address
        _db.Set<User>().Add(new User("Duy")
        {
            FirstName = "Duy",
            LastName = "Hoang",
            Addresses =
            {
                new Address
                {
                    Street = "12"
                }
            }
        });

        var count = await _db.SaveChangesAsync().ConfigureAwait(false);
        Assert.IsTrue(count >= 1);

        var users = await _db.Set<User>().ToListAsync().ConfigureAwait(false);

        Assert.IsTrue(users.Count >= 1);
        Assert.IsTrue(users.All(u => u.RowVersion != null));
    }

    [TestMethod]
    public async Task Test_Delete_WithSqlDb_Async()
    {
        await Test_Create_WithSqlDb_Async().ConfigureAwait(false);

        var user = await _db.Set<User>().Include(u=>u.Addresses).FirstAsync().ConfigureAwait(false);

        _db.RemoveRange(user.Addresses);
        _db.Remove(user);

        await _db.SaveChangesAsync().ConfigureAwait(false);

        var count = await _db.Set<User>().CountAsync(u => u.Id == user.Id).ConfigureAwait(false);

        Assert.IsTrue(count == 0);
    }

    [TestMethod]
    public async Task Test_Update_WithSqlDb_Async()
    {
        await Test_Create_WithSqlDb_Async().ConfigureAwait(false);

        var user = await _db.Set<User>().Include(u=>u.Addresses).FirstAsync().ConfigureAwait(false);

        user.FirstName = "Steven";
        user.Addresses.Last().Street = "Steven Street";

        await _db.SaveChangesAsync().ConfigureAwait(false);

        user = await _db.Set<User>().FirstAsync().ConfigureAwait(false);

        Assert.IsTrue(user.FirstName == "Steven");

        Assert.IsTrue(user.Addresses.Last().Street == "Steven Street");
    }
}