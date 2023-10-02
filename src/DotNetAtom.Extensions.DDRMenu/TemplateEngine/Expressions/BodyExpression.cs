using System.Collections.Generic;
using System.Threading.Tasks;
using WebFormsCore.UI;

namespace DotNetAtom.TemplateEngine.Expressions;

public sealed record BodyExpression : Expression
{
    public List<Expression> Expressions { get; } = new();

    public override async ValueTask WriteAsync(DdrMenu menu, IMenuItem item, HtmlTextWriter writer)
    {
        foreach (var expression in Expressions)
        {
            await expression.WriteAsync(menu, item, writer);
        }
    }
}