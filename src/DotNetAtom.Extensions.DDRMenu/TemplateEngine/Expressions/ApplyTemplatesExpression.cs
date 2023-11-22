using System.Collections.Generic;
using System.Threading.Tasks;
using DotNetAtom.Entities.Portals;
using WebFormsCore.UI;

namespace DotNetAtom.TemplateEngine.Expressions;

public sealed record ApplyTemplatesExpression(string Node, string? Mode = null) : Expression
{
    private readonly string _templateName = Mode is null ? Node : $"{Node}-{Mode}";

    public override async ValueTask WriteAsync(DdrMenu menu, IMenuItem item, HtmlTextWriter writer, IPortalSettings settings)
    {
        if (!menu.Templates.TryGetValue(_templateName, out var template))
        {
            await writer.WriteEncodedTextAsync($"<!-- Template for node '{Node}' with mode '{Mode}' not found -->");
            return;
        }

        var value = item.GetNode(Node, settings);

        if (value is not IEnumerable<IMenuItem> children)
        {
            return;
        }

        foreach (var child in children)
        {
            foreach (var expression in template.Expressions)
            {
                await expression.WriteAsync(menu, child, writer, settings);
            }
        }
    }
}