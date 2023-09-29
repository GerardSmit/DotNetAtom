using System.Collections;
using System.Collections.Generic;
using DotNetAtom.Entities;
using DotNetAtom.Tabs.Routes;
using HttpStack;

namespace DotNetAtom.Tabs.Collections;

internal class RouteCollection : IRouteCollection
{
    private readonly List<ITabRoute> _routes = new();

    public void Add(ITabInfo tab)
    {
        var path = tab.TabPath.Replace("//", "/");

        _routes.Add(new TabRoute(tab.TabId, new PathString[]
        {
            path,
            $"{path}/",
            $"{path}.aspx"
        }));
    }

    public void AddHome(int tabId)
    {
        _routes.Add(new TabRoute(tabId, new PathString[]
        {
            "/",
            "/Default.aspx"
        }));
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

    public ITabRoute this[int index] => _routes[index];

    public bool TryMatch(PathString path, out RouteMatch match)
    {
        foreach (var route in _routes)
        {
            if (route.IsMatch(path, out match))
            {
                return true;
            }
        }

        match = default;
        return false;
    }

    public bool TryGetPath(int tabId, out PathString path)
    {
        foreach (var route in _routes)
        {
            if (route.TryGetPath(tabId, out path))
            {
                return true;
            }
        }

        path = default;
        return false;
    }
}
