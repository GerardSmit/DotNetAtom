using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using DotNetAtom.Entities;
using HttpStack;

namespace DotNetAtom.Tabs;

public interface IRouteCollection : IReadOnlyList<ITabRoute>
{
    int? PortalId { get; }

    string? CultureCode { get; }

    int? HomeTabId { get; }

    bool TryMatch(IHttpRequest request, [NotNullWhen(true)] out ITabRoute? match);

    bool TryGetPath(ITabInfo tabInfo, out string? path);

    bool TryGetById(int id, [NotNullWhen(true)] out ITabRoute? match);
}