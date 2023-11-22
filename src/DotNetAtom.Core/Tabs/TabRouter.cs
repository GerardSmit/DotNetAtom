using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using DotNetAtom.Entities;
using DotNetAtom.Portals;
using DotNetAtom.Primitives;
using DotNetAtom.Tabs.Collections;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace DotNetAtom.Tabs;

internal record PortalRouteInformation(StringKey DefaultString,
    Dictionary<StringKey, RouteCollection> CollectionByCulture);

internal class TabRouter : ITabRouter
{
    private delegate bool TryGetDelegate<TKey, TValue>(TKey id, [NotNullWhen(true)] out TValue? match, RouteCollection tabs);

    private Dictionary<int, PortalRouteInformation> _tabsByPortal = new();
    private Dictionary<StringKey, RouteCollection> _globalTabsByCulture = new();
    private readonly ITabService _tabService;
    private readonly IPortalService _portalService;
    private readonly ILogger<TabRouter> _logger;
    private SimpleChangeToken _changeToken = new();

    public TabRouter(ITabService tabService, IPortalService portalService, ILogger<TabRouter> logger)
    {
        _tabService = tabService;
        _portalService = portalService;
        _logger = logger;
    }

    public IChangeToken ChangeToken => _changeToken;

    public IReadOnlyList<ITabRoute> GetChildren(int? portalId, string? cultureCode, ITabRoute? parent = null)
    {
        var items = new List<ITabRoute>();
        var state = new KeyValuePair<List<ITabRoute>, ITabRoute?>(items, parent);
        TryGet(portalId, cultureCode, state, out ITabRoute? _, Getter);
        return items;

        static bool Getter(KeyValuePair<List<ITabRoute>, ITabRoute?> kv, [NotNullWhen(true)] out ITabRoute? match, IRouteCollection tabs)
        {
            foreach (var tab in tabs)
            {
                if (tab.Parent != kv.Value)
                {
                    continue;
                }

                var shouldAdd = true;

                if (tab.Tab.TabId.HasValue)
                {
                    foreach (var result in kv.Key)
                    {
                        if (result.Tab.TabId == tab.Tab.TabId)
                        {
                            // Already added by another culture
                            shouldAdd = false;
                            break;
                        }
                    }
                }
                else
                {
                    foreach (var result in kv.Key)
                    {
                        if (result.Tab.TabPath.Equals(tab.Tab.TabPath, StringComparison.OrdinalIgnoreCase))
                        {
                            // Already added by another culture
                            shouldAdd = false;
                            break;
                        }
                    }
                }

                if (shouldAdd)
                {
                    kv.Key.Add(tab);
                }
            }

            match = null;
            return false;
        }
    }

    public bool Match(int? portalId, string? cultureCode, string path, [NotNullWhen(true)] out ITabRoute? match)
    {
        return TryGet(portalId, cultureCode, path, out match, Getter);

        static bool Getter(string path, [NotNullWhen(true)] out ITabRoute? match, IRouteCollection tabs)
        {
            return tabs.TryMatch(path, out match);
        }
    }

    public bool TryGetHomeTab(int? portalId, string? cultureCode, [NotNullWhen(true)] out ITabRoute? homeTab)
    {
        if (!TryGet(portalId, cultureCode, (object?)null, out int? homeId, Getter))
        {
            homeTab = default;
            return false;
        }

        return TryGetById(portalId, cultureCode, homeId.Value, out homeTab);

        static bool Getter(object? state, [NotNullWhen(true)] out int? homeId, IRouteCollection tabs)
        {
            homeId = tabs.HomeTabId;
            return homeId.HasValue;
        }
    }

    public bool TryGetById(int? portalId, string? cultureCode, int id, [NotNullWhen(true)] out ITabRoute? match)
    {
        return TryGet(portalId, cultureCode, id, out match, Getter);

        static bool Getter(int id, [NotNullWhen(true)] out ITabRoute? match, IRouteCollection tabs)
        {
            return tabs.TryGetById(id, out match);
        }
    }

    public bool TryGetByPath(int? portalId, string? cultureCode, string path, [NotNullWhen(true)] out ITabRoute? match)
    {
        return TryGetByPath(portalId, cultureCode, path.AsMemory(), out match);
    }

    public bool TryGetByPath(int? portalId, string? cultureCode, ReadOnlyMemory<char> path, [NotNullWhen(true)] out ITabRoute? match)
    {
        return TryGet(portalId, cultureCode, path, out match, Getter);

        static bool Getter(ReadOnlyMemory<char> path, [NotNullWhen(true)] out ITabRoute? match, IRouteCollection tabs)
        {
            var span = path.Span;

            foreach (var tab in tabs)
            {
                if (tab.Tab.TabPath.AsSpan().Equals(span, StringComparison.OrdinalIgnoreCase))
                {
                    match = tab;
                    return true;
                }
            }

            match = default;
            return false;
        }
    }

    public bool TryGetPath(int? portalId, string? cultureCode, ITabInfo tabInfo, [NotNullWhen(true)] out string? path)
    {
        return TryGet(portalId, cultureCode, tabInfo, out path, Getter);

        static bool Getter(ITabInfo tabInfo, [NotNullWhen(true)] out string? path, IRouteCollection tabs)
        {
            return tabs.TryGetPath(tabInfo, out path);
        }
    }

    public Task LoadAsync()
    {
        var portalTabs = new Dictionary<int, PortalRouteInformation>();
        var tabsByCulture = new Dictionary<StringKey, RouteCollection>(StringKeyComparer.OrdinalIgnoreCase);
        var allCollections = new List<RouteCollection>();

        // Add tabs by culture
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
            }
            else
            {
                tabs = tabInformation.CollectionByCulture;
            }

            if (!tabs.TryGetValue(cultureCode, out var tabCollection))
            {
                tabCollection = new RouteCollection(tab.PortalId, tab.CultureCode);
                tabs.Add(cultureCode, tabCollection);
                allCollections.Add(tabCollection);
            }

            tabCollection.Add(tab);
        }

        // Set home tabs
        foreach (var kv in portalTabs)
        {
            foreach (var localization in _portalService.GetPortalCultures(kv.Key))
            {
                if (localization.HomeTabId == -1)
                {
                    continue;
                }

                if (!kv.Value.CollectionByCulture.TryGetValue(localization.CultureCode, out var collection))
                {
                    collection = new RouteCollection(kv.Key, localization.CultureCode);
                    kv.Value.CollectionByCulture.Add(localization.CultureCode, collection);
                }

                collection.HomeTabId = localization.HomeTabId;
            }
        }

        _tabsByPortal = portalTabs;
        _globalTabsByCulture = tabsByCulture;

        // Set parents
#if NET
        var slashSlash = "//";
#else
        ReadOnlySpan<char> slashSlash = stackalloc char[] { '/', '/' };
#endif

        foreach (var collection in allCollections)
        {
            foreach (var route in collection)
            {
                ITabRoute? parent;

                // Database parent
                if (route.Tab.ParentId.HasValue)
                {
                    if (TryGetById(route.Tab.PortalId, route.Tab.CultureCode, route.Tab.ParentId.Value, out parent))
                    {
                        route.Parent = parent;
                    }

                    continue;
                }

                // Virtual parent
                var span = route.Tab.TabPath.AsSpan();
                var index = span.LastIndexOf(slashSlash);
                var firstIndex = span.IndexOf(slashSlash);

                if (index == -1 || index == firstIndex)
                {
                    // No parent
                    continue;
                }

                var parentPath = route.Tab.TabPath.AsMemory(0, index);

                if (TryGetByPath(route.Tab.PortalId, route.Tab.CultureCode, parentPath, out parent))
                {
                    route.Parent = parent;
                }
                else
                {
                    _logger.LogWarning("Parent tab not found for {TabPath} in portal {PortalId} with culture code {CultureCode}", route.Tab.TabPath, collection.PortalId, collection.CultureCode);
                }
            }
        }

        var currentToken = _changeToken;
        _changeToken = new SimpleChangeToken();
        currentToken.OnChange();

        return Task.CompletedTask;
    }

    private bool TryGet<TState, TValue>(int? portalId, string? cultureCode, TState state, [NotNullWhen(true)] out TValue? match, TryGetDelegate<TState, TValue> getter)
    {
        StringKey cultureKey = cultureCode ?? default(StringKey);
        RouteCollection? tabs;

        // Try to match portal-specific tabs
        if (portalId.HasValue && _tabsByPortal.TryGetValue(portalId.Value, out var portalInfo))
        {
            // Try to match portal-specific tabs by culture
            if (portalInfo.CollectionByCulture.TryGetValue(cultureKey, out tabs) &&
                getter(state, out match, tabs))
            {
                return true;
            }

            // Try to match portal-specific tabs by default culture
            if (portalInfo.DefaultString != cultureKey &&
                portalInfo.CollectionByCulture.TryGetValue(portalInfo.DefaultString, out tabs) &&
                getter(state, out match, tabs))
            {
                return true;
            }

            // Try to match portal-specific tabs by invariant culture
            if (portalInfo.CollectionByCulture.TryGetValue(default, out tabs) &&
                getter(state, out match, tabs))
            {
                return true;
            }
        }

        // Try to match global tabs by the current culture
        if (_globalTabsByCulture.TryGetValue(cultureKey, out tabs) &&
            getter(state, out match, tabs))
        {
            return true;
        }

        // Try to match global tabs by invariant culture
        if (_globalTabsByCulture.TryGetValue(default, out tabs) &&
            getter(state, out match, tabs))
        {
            return true;
        }

        match = default;
        return false;
    }
}
