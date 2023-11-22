using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using DotNetAtom.Entities;
using Microsoft.Extensions.Primitives;

namespace DotNetAtom.Tabs;

public interface ITabRouter
{
    IChangeToken ChangeToken { get; }

    IReadOnlyList<ITabRoute> GetChildren(int? portalId, string? cultureCode, ITabRoute? parent);

    bool Match(int? portalId, string? cultureCode, string path, [NotNullWhen(true)] out ITabRoute? match);

    bool TryGetHomeTab(int? portalId, string? cultureCode, [NotNullWhen(true)] out ITabRoute? homeTab);

    bool TryGetById(int? portalId, string? cultureCode, int id, [NotNullWhen(true)] out ITabRoute? match);

    bool TryGetPath(int? portalId, string? cultureCode, ITabInfo tabInfo, [NotNullWhen(true)] out string? path);

    Task LoadAsync();
}