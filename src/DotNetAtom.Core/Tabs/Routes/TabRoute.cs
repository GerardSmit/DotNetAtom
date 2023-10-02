using System;
#if NET8_0_OR_GREATER
using System.Collections.Frozen;
#endif
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using DotNetAtom.Entities;
using HttpStack;

namespace DotNetAtom.Tabs.Routes;

[DebuggerDisplay("Path: {" + nameof(DisplayPath) + "}")]
internal class TabRoute : ITabRoute
{
#if NET8_0_OR_GREATER
    private readonly FrozenSet<string> _paths;
#else
    private readonly List<string> _paths;
#endif

    public ITabInfo Tab { get; }

    public ITabRoute? Parent { get; set; }

    private string DisplayPath => _paths.FirstOrDefault() ?? "";

    public TabRoute(ITabInfo tabInfo, IEnumerable<PathString> paths)
    {
        var stringValues = paths.Where(i => i.HasValue).Select(i => i.Value!);

        Tab = tabInfo;

#if NET8_0_OR_GREATER
        _paths = stringValues.ToFrozenSet(StringComparer.OrdinalIgnoreCase);
#else
        _paths = new List<string>(stringValues);
#endif
    }

    public bool IsMatch(IHttpRequest request)
    {
        if (!request.Path.HasValue)
        {
            return false;
        }

#if NET8_0_OR_GREATER
        return _paths.Contains(request.Path.Value!);
#else
        return _paths.Contains(request.Path.Value!, StringComparer.OrdinalIgnoreCase);
#endif
    }

    public bool TryGetPath([NotNullWhen(true)] out string? path)
    {
        path = _paths.FirstOrDefault();
        return path != null;
    }
}
