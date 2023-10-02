using System;
using System.Threading;
using System.Threading.Tasks;
using DotNetAtom.Framework;
using WebFormsCore.UI;

namespace DotNetAtom.DesktopModules.HTML;

public partial class EditHTML : PortalModuleBase
{
    protected override async ValueTask OnLoadAsync(CancellationToken token)
    {
        await base.OnLoadAsync(token);

        if (!Page.IsPostBack)
        {
            editor.Text = ModuleContext.Configuration.HtmlContent ?? "";
        }
    }
}
