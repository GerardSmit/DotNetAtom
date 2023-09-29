using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DotNetAtom.Portals;

public class PortalRepository : IPortalRepository
{
    private readonly IDbContextFactory<AtomDbContext> _dbContextFactory;

    public PortalRepository(IDbContextFactory<AtomDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<IReadOnlyList<IPortalInfo>> GetPortalsAsync()
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        return await dbContext.PortalLocalization
            .Include(e => e.Portal.Settings)
            .Include(e => e.Portal.Administrator)
            .Include(e => e.Portal.AdministratorRole)
            .Include(e => e.Portal.RegisteredRole)
            .AsSplitQuery()
            .AsAsyncEnumerable()
            .Select(PortalInfo.FromEntity)
            .ToArrayAsync();
    }
}
