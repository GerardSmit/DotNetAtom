using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using DotNetAtom.Entities;
using DotNetAtom.Tabs.Routes;
using HttpStack;

namespace DotNetAtom.Tabs.Collections;

internal class RouteCollection : IRouteCollection
{
    private readonly List<ITabRoute> _routes = new();
    private Dictionary<int, ITabRoute> _routesById = new();

    public RouteCollection(int? portalId, string? cultureCode)
    {
        PortalId = portalId;
        CultureCode = cultureCode;
    }

    public ITabRoute? HomeTab { get; set; }

    public void Add(ITabInfo tab)
    {
        var path = tab.TabPath.Replace("//", "/");
        var route = new TabRoute(tab, new PathString[]
        {
            path,
            $"{path}/",
            $"{path}.aspx"
        });

        _routes.Add(route);

        if (tab.TabId.HasValue)
        {
            _routesById.Add(tab.TabId.Value, route);
        }
    }

    public void Add(ITabRoute route)
    {
        _routes.Add(route);
    }

    public IEnumerator<ITabRoute> GetEnumerator()
    {
        return _routes.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public int Count => _routes.Count;

    public int? PortalId { get; }

    public string? CultureCode { get; }

    public int? HomeTabId { get; set; }

    public ITabRoute this[int index] => _routes[index];

    public bool TryMatch(IHttpRequest request, [NotNullWhen(true)] out ITabRoute? match)
    {
        foreach (var route in _routes)
        {
            if (route.IsMatch(request))
            {
                match = route;
                return true;
            }
        }

        match = default;
        return false;
    }

    public bool TryGetPath(ITabInfo tabInfo, [NotNullWhen(true)] out string? path)
    {
        foreach (var route in _routes)
        {
            if (!route.Tab.Equals(tabInfo))
            {
                continue;
            }

            if (route.TryGetPath(out path))
            {
                return true;
            }
        }

        path = default;
        return false;
    }

    public bool TryGetById(int id, [NotNullWhen(true)] out ITabRoute? match)
    {
        return _routesById.TryGetValue(id, out match);
    }
}
