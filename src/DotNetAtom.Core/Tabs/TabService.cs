using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetAtom.Entities;

namespace DotNetAtom.Tabs;

internal class TabService : ITabService
{
    private readonly ITabRepository _tabRepository;
    private Dictionary<int, ITabInfo> _tabs = new();

    public TabService(ITabRepository tabRepository)
    {
        _tabRepository = tabRepository;
    }

    public IReadOnlyCollection<ITabInfo> Tabs => _tabs.Values;

    public ITabInfo GetTab(int tabId)
    {
        return _tabs.TryGetValue(tabId, out var tab)
            ? tab
            : throw new KeyNotFoundException($"Tab {tabId} not found");
    }

    public async Task LoadAsync()
    {
        _tabs = (await _tabRepository.GetTabsAsync()).ToDictionary(t => t.TabId, t => t);
    }
}
