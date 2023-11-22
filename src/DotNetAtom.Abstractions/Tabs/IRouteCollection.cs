using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using DotNetAtom.Entities;

namespace DotNetAtom.Tabs;

public interface IRouteCollection : IReadOnlyList<ITabRoute>
{
    int? PortalId { get; }

    string? CultureCode { get; }

    int? HomeTabId { get; }

    bool TryMatch(string path, [NotNullWhen(true)] out ITabRoute? match);

    bool TryGetPath(ITabInfo tabInfo, out string? path);

    bool TryGetById(int id, [NotNullWhen(true)] out ITabRoute? match);
}