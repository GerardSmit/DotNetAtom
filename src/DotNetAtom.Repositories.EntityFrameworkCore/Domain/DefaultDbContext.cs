using Microsoft.EntityFrameworkCore;

namespace DotNetAtom.Database;

public class DefaultDbContext : AtomDbContext
{
    public DefaultDbContext(DbContextOptions<DefaultDbContext> options)
        : base(options)
    {
    }
}
