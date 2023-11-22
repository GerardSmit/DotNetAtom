using System.Threading.Tasks;
using DotNetAtom.Entities.Portals;
using WebFormsCore.UI;

namespace DotNetAtom.TemplateEngine.Expressions;

public sealed record TestExpression(string Node, Expression Choose, Expression? Otherwise = null) : Expression
{
    public override ValueTask WriteAsync(DdrMenu menu, IMenuItem item, HtmlTextWriter writer, IPortalSettings settings)
    {
        if (item.TestNode(Node, settings))
        {
            return Choose.WriteAsync(menu, item, writer, settings);
        }

        if (Otherwise is not null)
        {
            return Otherwise.WriteAsync(menu, item, writer, settings);
        }

        return default;
    }
}