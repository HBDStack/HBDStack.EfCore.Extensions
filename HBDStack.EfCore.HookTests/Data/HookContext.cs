using Microsoft.EntityFrameworkCore;

namespace HBDStack.EfCore.HookTests.Data;

public class HookContext : DbContext
{
    public HookContext(DbContextOptions<HookContext> options) :
        base(options)
    {
    }
}