using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetAtom.Entities;
using DotNetAtom.Tabs;
using DotNetAtom.Tabs.Cache;
using Microsoft.EntityFrameworkCore;

namespace DotNetAtom.EntityFrameworkCore.Repositories.Tabs;

public class TabRepository : ITabRepository
{
    private readonly IDbContextFactory<AtomDbContext> _dbContextFactory;

    public TabRepository(IDbContextFactory<AtomDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<IReadOnlyList<ITabInfo>> GetTabsAsync()
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        return await dbContext.Tab
            .Include(t => t.TabSettings)
            .Include(t => t.TabPermissions)
            .Include(t => t.TabModules).ThenInclude(tm => tm.ModuleSettings)
            .Include(t => t.TabModules).ThenInclude(tm => tm.Module).ThenInclude(m => m.HtmlTexts)
            .Include(t => t.TabModules).ThenInclude(tm => tm.Module).ThenInclude(m => m.Permissions)
            .OrderBy(t => t.PortalId)
            .AsSplitQuery()
            .AsAsyncEnumerable()
            .Select(i => (ITabInfo) new TabInfo(i))
            .ToListAsync();
    }
}
