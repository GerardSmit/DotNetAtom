using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using DotNetAtom.Entities;
using HttpStack;

namespace DotNetAtom.Tabs;

public interface ITabRouter
{
    IReadOnlyList<ITabRoute> GetChildren(int? portalId, string? cultureCode, ITabRoute? parent);

    bool Match(int? portalId, string? cultureCode, IHttpRequest request, [NotNullWhen(true)] out ITabRoute? match);

    bool TryGetHomeTab(int? portalId, string? cultureCode, [NotNullWhen(true)] out ITabRoute? homeTab);

    bool TryGetById(int? portalId, string? cultureCode, int id, [NotNullWhen(true)] out ITabRoute? match);

    bool TryGetPath(int? portalId, string? cultureCode, ITabInfo tabInfo, [NotNullWhen(true)] out string? path);

    Task LoadAsync();
}