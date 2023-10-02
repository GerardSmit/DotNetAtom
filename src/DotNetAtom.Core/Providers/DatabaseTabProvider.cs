using System.Collections.Generic;
using System.Threading.Tasks;
using DotNetAtom.Entities;
using DotNetAtom.Tabs;

namespace DotNetAtom.Providers;

public class DatabaseTabProvider(ITabRepository repository) : ITabProvider
{
    public async ValueTask<IReadOnlyList<ITabInfo>> GetTabs()
    {
        return await repository.GetTabsAsync();
    }
}
