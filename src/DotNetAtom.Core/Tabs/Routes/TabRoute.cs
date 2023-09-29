using System;
#if NET8_0_OR_GREATER
using System.Collections.Frozen;
#endif
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using HttpStack;

namespace DotNetAtom.Tabs.Routes;

[DebuggerDisplay("Path: {" + nameof(DisplayPath) + "} TabId: {" + nameof(_tabId) + "}")]
internal class TabRoute : ITabRoute
{
    private readonly int _tabId;
#if NET8_0_OR_GREATER
    private readonly FrozenSet<string> _paths;
#else
    private readonly List<string> _paths;
#endif

    private string DisplayPath => _paths.FirstOrDefault() ?? "";

    public TabRoute(int tabId, IEnumerable<PathString> paths)
    {
        var stringValues = paths.Where(i => i.HasValue).Select(i => i.Value!);

        _tabId = tabId;

#if NET8_0_OR_GREATER
        _paths = stringValues.ToFrozenSet(StringComparer.OrdinalIgnoreCase);
#else
        _paths = new List<string>(stringValues);
#endif
    }

    public bool IsMatch(PathString fullPath, out RouteMatch match)
    {
        if (!fullPath.HasValue)
        {
            match = default;
            return false;
        }

#if NET8_0_OR_GREATER
        if (_paths.Contains(fullPath.Value!))
#else
        if (_paths.Contains(fullPath.Value!, StringComparer.OrdinalIgnoreCase))
#endif
        {
            match = new RouteMatch(_tabId, fullPath.Value!);
            return true;
        }

        match = default;
        return false;
    }

    public bool TryGetPath(int tabId, out PathString path)
    {
        if (_tabId == tabId && _paths.Count > 0)
        {
            path = _paths.First();
            return true;
        }

        path = default;
        return false;
    }
}
