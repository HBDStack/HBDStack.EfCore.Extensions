using Microsoft.EntityFrameworkCore;

namespace DataLayer;

public class MyDbContext : DbContext
{
    #region Constructors

    public MyDbContext(DbContextOptions options) : base(options)
    {
    }

    #endregion Constructors
}