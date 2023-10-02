using System.Collections.Generic;
using System.Threading.Tasks;
using WebFormsCore.UI;

namespace DotNetAtom.TemplateEngine.Expressions;

public sealed record ApplyExpression(string Node, BodyExpression Template) : Expression
{
    public override async ValueTask WriteAsync(DdrMenu menu, IMenuItem item, HtmlTextWriter writer)
    {
        var value = item.GetNode(Node);

        if (value is not IEnumerable<IMenuItem> children)
        {
            return;
        }

        foreach (var child in children)
        {
            foreach (var expression in Template.Expressions)
            {
                await expression.WriteAsync(menu, child, writer);
            }
        }
    }
}
