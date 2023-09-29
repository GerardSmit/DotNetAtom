using System.Threading;
using System.Threading.Tasks;
using DotNetAtom.Entities;
using DotNetAtom.Modules;
using Microsoft.Extensions.DependencyInjection;
using WebFormsCore.UI;

namespace DotNetAtom.UI.Skins;

public class ModuleContainer : Control
{
    public IModuleInfo ModuleInfo { get; set; } = null!;

    public override async ValueTask RenderAsync(HtmlTextWriter writer, CancellationToken token)
    {
        var moduleService = Context.RequestServices.GetRequiredService<IModuleService>();
        var module = moduleService.GetDefinition(ModuleInfo.ModuleDefinitionId);

        await writer.WriteAsync("<div class=\"DnnModule DnnModule-");
        await writer.WriteAsync(module.DesktopModule.ModuleName);
        await writer.WriteAsync(" DnnModule-");
        await writer.WriteObjectAsync(ModuleInfo.ModuleId);
        await writer.WriteAsync("\">");

        await writer.WriteAsync("<a name=\"");
        await writer.WriteObjectAsync(ModuleInfo.ModuleId);
        await writer.WriteAsync("\"></a>");

        await base.RenderChildrenAsync(writer, token);
        await writer.WriteAsync("</div>");
    }
}
