using System.Diagnostics.CodeAnalysis;
using DotNetAtom.Portals;

namespace DotNetAtom.Skins;

public interface ISkinService
{
    [return: NotNullIfNotNull(nameof(skinSrc))]
    string? GetSkinSrc(string? skinSrc, IPortalInfo portalInfo);
}
