using System.Collections.Generic;
using DotNetAtom.Entities.Portals;

namespace DotNetAtom.TemplateEngine.Items;

public class RootItem(IReadOnlyCollection<IMenuItem> children)
    : IMenuItem
{
    public virtual object? GetNode(string key, IPortalSettings settings)
    {
        return key switch
        {
            "NODE" => children,
            _ => null
        };
    }

    public virtual bool TestNode(string key, IPortalSettings settings)
    {
        return key switch
        {
            "NODE" => children.Count > 0,
            _ => GetNode(key, settings) is not null
        };
    }
}