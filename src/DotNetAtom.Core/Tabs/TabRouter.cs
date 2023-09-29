using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DotNetAtom.Portals;
using DotNetAtom.Tabs.Collections;
using DotNetAtom.Tabs.Routes;
using HttpStack;

namespace DotNetAtom.Tabs;

internal record PortalRouteInformation(StringKey DefaultString, Dictionary<StringKey, RouteCollection> CollectionByCulture);

internal class TabRouter : ITabRouter
{
    private Dictionary<int, PortalRouteInformation> _tabsByPortal = new();
    private Dictionary<StringKey, RouteCollection> _globalTabsByCulture = new();
    private readonly ITabService _tabService;
    private readonly IPortalService _portalService;

    public TabRouter(ITabService tabService, IPortalService portalService)
    {
        _tabService = tabService;
        _portalService = portalService;
    }

   public IRouteCollection GetTabCollection(int portalId, string? cultureCode = null)
    {
        var cultureKey = cultureCode ?? default(StringKey);

        if (!_tabsByPortal.TryGetValue(portalId, out var tabInformation))
        {
            throw new KeyNotFoundException($"Portal {portalId} not found");
        }

        if (!tabInformation.CollectionByCulture.TryGetValue(cultureKey, out var tabs))
        {
            throw new KeyNotFoundException($"Portal {portalId} not found");
        }

        return tabs;
    }

    public bool Match(int? portalId, string? cultureCode, PathString path, out RouteMatch match)
    {
        var cultureKey = cultureCode ?? default(StringKey);

        // Try to match portal-specific tabs
        RouteCollection? tabs;

        if (portalId.HasValue && _tabsByPortal.TryGetValue(portalId.Value, out var portalInfo))
        {
            if (portalInfo.CollectionByCulture.TryGetValue(cultureKey, out tabs) &&
                TryMatchTab(path, out match, tabs))
            {
                return true;
            }

            if (portalInfo.DefaultString != cultureKey &&
                portalInfo.CollectionByCulture.TryGetValue(portalInfo.DefaultString, out tabs) &&
                TryMatchTab(path, out match, tabs))
            {
                return true;
            }

            if (portalInfo.CollectionByCulture.TryGetValue(default, out tabs) &&
                TryMatchTab(path, out match, tabs))
            {
                return true;
            }
        }

        // Try to match global tabs
        if (_globalTabsByCulture.TryGetValue(cultureKey, out tabs) &&
            TryMatchTab(path, out match, tabs))
        {
            return true;
        }

        if (_globalTabsByCulture.TryGetValue(default, out tabs)  &&
            TryMatchTab(path, out match, tabs))
        {
            return true;
        }

        match = default;
        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool TryMatchTab(PathString path, out RouteMatch match, IRouteCollection tabs)
    {
        return tabs.TryMatch(path, out match);
    }

    public bool TryGetPath(int? portalId, string? cultureCode, int tabId, out PathString match)
    {
        var cultureKey = cultureCode ?? default(StringKey);

        // Try to match portal-specific tabs
        RouteCollection? tabs;

        if (portalId.HasValue && _tabsByPortal.TryGetValue(portalId.Value, out var portalInfo))
        {
            if (portalInfo.CollectionByCulture.TryGetValue(cultureKey, out tabs) &&
                TryGetPath(tabId, out match, tabs))
            {
                return true;
            }

            if (portalInfo.DefaultString != cultureKey &&
                portalInfo.CollectionByCulture.TryGetValue(portalInfo.DefaultString, out tabs) &&
                TryGetPath(tabId, out match, tabs))
            {
                return true;
            }

            if (portalInfo.CollectionByCulture.TryGetValue(default, out tabs) &&
                TryGetPath(tabId, out match, tabs))
            {
                return true;
            }
        }

        // Try to match global tabs
        if (_globalTabsByCulture.TryGetValue(cultureKey, out tabs) &&
            TryGetPath(tabId, out match, tabs))
        {
            return true;
        }

        if (_globalTabsByCulture.TryGetValue(default, out tabs)  &&
            TryGetPath(tabId, out match, tabs))
        {
            return true;
        }

        match = default;
        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool TryGetPath(int tabId, out PathString match, IRouteCollection tabs)
    {
        return tabs.TryGetPath(tabId, out match);
    }

    public Task LoadAsync()
    {
        var portalTabs = new Dictionary<int, PortalRouteInformation>();
        var tabsByCulture = new Dictionary<StringKey, RouteCollection>(StringKeyComparer.OrdinalIgnoreCase);

        foreach (var tab in _tabService.Tabs)
        {
            var cultureCode = tab.CultureCode;
            Dictionary<StringKey, RouteCollection> tabs;

            if (tab.PortalId is not { } portalId)
            {
                tabs = tabsByCulture;
            }
            else if (!portalTabs.TryGetValue(portalId, out var tabInformation))
            {
                var portal = _portalService.GetPortal(portalId);

                tabs = new Dictionary<StringKey, RouteCollection>(StringKeyComparer.OrdinalIgnoreCase);
                tabInformation = new PortalRouteInformation(portal.DefaultLanguage, tabs);
                portalTabs.Add(portalId, tabInformation);

                foreach (var localization in _portalService.GetPortalCultures(portalId))
                {
                    var collection = new RouteCollection();
                    tabs.Add(localization.CultureCode, collection);

                    if (localization.HomeTabId != -1)
                    {
                        collection.AddHome(localization.HomeTabId);
                    }
                }
            }
            else
            {
                tabs = tabInformation.CollectionByCulture;
            }

            if (!tabs.TryGetValue(cultureCode, out var tabCollection))
            {
                tabCollection = new RouteCollection();
                tabs.Add(cultureCode, tabCollection);
            }

            tabCollection.Add(tab);
        }

        _tabsByPortal = portalTabs;
        _globalTabsByCulture = tabsByCulture;

        return Task.CompletedTask;
    }
}
