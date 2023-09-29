using System.Collections.Generic;
using System.Threading.Tasks;
using DotNetAtom.Entities;

namespace DotNetAtom.Tabs;

public interface ITabRepository
{
    Task<IReadOnlyList<ITabInfo>> GetTabsAsync();
}
