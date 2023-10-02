using System.Threading;
using System.Threading.Tasks;
using DotNetAtom.Framework;
using WebFormsCore.UI;

namespace DotNetAtom.DesktopModules.HTML;

public partial class HtmlModule : PortalModuleBase
{
    public override ValueTask RenderAsync(HtmlTextWriter writer, CancellationToken token)
    {
        if (ModuleContext.Configuration.HtmlContent is {} htmlContent)
        {
            return writer.WriteAsync(htmlContent);
        }

        return default;
    }
}
