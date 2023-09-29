using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetAtom.Entities;
using DotNetAtom.Modules;
using DotNetAtom.Tabs.Cache;
using Microsoft.EntityFrameworkCore;

namespace DotNetAtom.Infrastructure.EntityFrameworkCore;

public class ModuleRepository : IModuleRepository
{
    private readonly IDbContextFactory<AtomDbContext> _dbContextFactory;

    public ModuleRepository(IDbContextFactory<AtomDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<IReadOnlyDictionary<int, IModuleDefinitionInfo>> GetDefinitionsAsync()
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        return await dbContext.ModuleDefinition
            .Include(md => md.DesktopModule)
            .Include(md => md.ModuleControls)
            .AsAsyncEnumerable()
            .ToDictionaryAsync(md => md.Id, md => (IModuleDefinitionInfo) new ModuleDefinitionInfo(md));
    }
}
