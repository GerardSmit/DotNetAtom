using DotNetAtom.Entities.Portals;

namespace DotNetAtom.TemplateEngine;

public interface IMenuItem
{
    object? GetNode(string key, IPortalSettings settings);

    bool TestNode(string key, IPortalSettings settings);
}