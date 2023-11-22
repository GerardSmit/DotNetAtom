using System.Collections.Generic;
using DotNetAtom.Entities;
using DotNetAtom.Entities.Portals;
using DotNetAtom.Portals;

namespace DotNetAtom.TemplateEngine.Items;

public class TabItem(ITabInfo tabInfo, IReadOnlyCollection<IMenuItem> children, string? url)
    : RootItem(children)
{
    public override object? GetNode(string key, IPortalSettings settings)
    {
        return key switch
        {
            "TEXT" => tabInfo.TabName,
            "URL" => url,
            _ => base.GetNode(key, settings)
        };
    }

    public override bool TestNode(string key, IPortalSettings settings)
    {
        return key switch
        {
            "ENABLED" => !tabInfo.DisableLink,
            "SELECTED" => settings.ActiveTab != null && settings.ActiveTab.Equals(tabInfo),
            _ => base.TestNode(key, settings)
        };
    }
}
