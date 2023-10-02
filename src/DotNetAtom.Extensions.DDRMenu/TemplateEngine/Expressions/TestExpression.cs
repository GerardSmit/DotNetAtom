using System.Threading.Tasks;
using WebFormsCore.UI;

namespace DotNetAtom.TemplateEngine.Expressions;

public sealed record TestExpression(string Node, Expression Choose, Expression? Otherwise = null) : Expression
{
    public override ValueTask WriteAsync(DdrMenu menu, IMenuItem item, HtmlTextWriter writer)
    {
        if (item.TestNode(Node))
        {
            return Choose.WriteAsync(menu, item, writer);
        }

        if (Otherwise is not null)
        {
            return Otherwise.WriteAsync(menu, item, writer);
        }

        return default;
    }
}