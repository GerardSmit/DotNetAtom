using System.Threading.Tasks;
using WebFormsCore.UI;

namespace DotNetAtom.TemplateEngine.Expressions;

public sealed record NodeExpression(string Node) : Expression
{
    public override ValueTask WriteAsync(DdrMenu menu, IMenuItem item, HtmlTextWriter writer)
    {
        var value = item.GetNode(Node)?.ToString();

        if (value is not null)
        {
            return writer.WriteEncodedTextAsync(value);
        }

        return default;
    }
}