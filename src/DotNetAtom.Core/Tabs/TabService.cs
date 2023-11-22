using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetAtom.Entities;
using DotNetAtom.Portals;
using DotNetAtom.Providers;

namespace DotNetAtom.Tabs;

internal class TabService : ITabService
{
    private readonly IPortalService _portalService;
    private readonly IEnumerable<ITabProvider> _providers;
    private readonly List<ITabInfo> _tabs = new();
    private readonly Dictionary<int, ITabInfo> _tabsById = new();

    public TabService(IEnumerable<ITabProvider> providers, IPortalService portalService)
    {
        _providers = providers;
        _portalService = portalService;
    }

    public IReadOnlyCollection<ITabInfo> Tabs => _tabs;

    public ITabInfo GetTab(int tabId)
    {
        return _tabsById.TryGetValue(tabId, out var tab)
            ? tab
            : throw new KeyNotFoundException($"Tab {tabId} not found");
    }

    public async Task LoadAsync()
    {
        _tabs.Clear();
        _tabsById.Clear();

        foreach (var provider in _providers)
        {
            foreach (var portal in _portalService.Portals)
            {
                var tabs = await provider.GetTabs();
                _tabs.AddRange(tabs);

                foreach (var tab in tabs)
                {
                    if (tab.TabId.HasValue)
                    {
                        _tabsById.Add(tab.TabId.Value, tab);
                    }
                }
            }
        }
    }
}
