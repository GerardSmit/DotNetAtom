using System.Threading.Tasks;
using WebFormsCore.UI;

namespace DotNetAtom.TemplateEngine.Expressions;

public abstract record Expression
{
    public abstract ValueTask WriteAsync(DdrMenu menu, IMenuItem item, HtmlTextWriter writer);
}