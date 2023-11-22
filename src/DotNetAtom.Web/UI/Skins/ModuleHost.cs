using System.Threading;
using System.Threading.Tasks;
using DotNetAtom.Entities;
using DotNetAtom.Modules;
using DotNetAtom.UI.Containers;
using Microsoft.Extensions.DependencyInjection;
using WebFormsCore.UI;
using Container = System.ComponentModel.Container;

namespace DotNetAtom.UI.Skins;

public class ModuleHost : Control, IModuleHost
{
    public IModuleInfo ModuleInfo { get; set; } = null!;

    public IModuleControl ModuleControl { get; set; } = null!;

    public override async ValueTask RenderAsync(HtmlTextWriter writer, CancellationToken token)
    {
        var moduleService = Context.RequestServices.GetRequiredService<IModuleService>();
        var module = moduleService.GetDefinition(ModuleInfo);

        await writer.WriteAsync("<div class=\"DnnModule DnnModule-");
        await writer.WriteAsync(module.DesktopModule.ModuleName);

        if (ModuleInfo.ModuleId.HasValue)
        {
            await writer.WriteAsync(" DnnModule-");
            await writer.WriteObjectAsync(ModuleInfo.ModuleId);
        }

        await writer.WriteAsync("\">");

        await writer.WriteAsync("<a name=\"");
        await writer.WriteObjectAsync(ModuleInfo.ModuleId);
        await writer.WriteAsync("\"></a>");

        await base.RenderChildrenAsync(writer, token);
        await writer.WriteAsync("</div>");
    }
}
