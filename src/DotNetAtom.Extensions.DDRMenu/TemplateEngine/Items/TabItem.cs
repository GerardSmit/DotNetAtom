using System.Collections.Generic;
using DotNetAtom.Entities;

namespace DotNetAtom.TemplateEngine.Items;

public class TabItem(ITabInfo tabInfo, IReadOnlyCollection<IMenuItem> children, string? url, bool isActive)
    : RootItem(children)
{
    public override object? GetNode(string key)
    {
        return key switch
        {
            "TEXT" => tabInfo.TabName,
            "URL" => url,
            _ => base.GetNode(key)
        };
    }

    public override bool TestNode(string key)
    {
        return key switch
        {
            "ENABLED" => !tabInfo.DisableLink,
            "SELECTED" => isActive,
            _ => base.TestNode(key)
        };
    }
}
