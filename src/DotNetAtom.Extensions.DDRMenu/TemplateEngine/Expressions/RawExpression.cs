using System.Threading.Tasks;
using DotNetAtom.Entities.Portals;
using WebFormsCore.UI;

namespace DotNetAtom.TemplateEngine.Expressions;

public sealed record RawExpression(string Value) : Expression
{
    public override ValueTask WriteAsync(DdrMenu menu, IMenuItem item, HtmlTextWriter writer, IPortalSettings settings)
    {
        return writer.WriteAsync(Value);
    }
}