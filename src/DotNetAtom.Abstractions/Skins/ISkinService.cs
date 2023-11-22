using System.Diagnostics.CodeAnalysis;
using DotNetAtom.Entities;
using DotNetAtom.Portals;

namespace DotNetAtom.Skins;

public interface ISkinService
{
    [return: NotNullIfNotNull(nameof(skinSrc))]
    string? GetSkinSrc(string? skinSrc, IPortalInfo portalInfo);

    bool IsAdminSkin(ITabInfo tabInfo, int? moduleId, string? controlKey);
}
