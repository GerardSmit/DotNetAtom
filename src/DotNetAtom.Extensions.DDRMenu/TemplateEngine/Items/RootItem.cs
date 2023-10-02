using System.Collections.Generic;

namespace DotNetAtom.TemplateEngine.Items;

public class RootItem(IReadOnlyCollection<IMenuItem> children)
    : IMenuItem
{
    public virtual object? GetNode(string key)
    {
        return key switch
        {
            "NODE" => children,
            _ => null
        };
    }

    public virtual bool TestNode(string key)
    {
        return key switch
        {
            "NODE" => children.Count > 0,
            _ => GetNode(key) is not null
        };
    }
}