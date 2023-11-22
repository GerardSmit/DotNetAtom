using System.Threading.Tasks;
using DotNetAtom.Entities;
using DotNetAtom.Entities.Portals;
using WebFormsCore.UI;

namespace DotNetAtom.Modules.Factories;

public class WebFormsControlFactory : IModuleControlFactory
{
    public string Extension => ".ascx";

    public Task<Control?> CreateControlAsync(
        Page page,
        IPortalSettings settings,
        IModuleInfo module,
        string? controlKey,
        string controlSrc)
    {
        var control = page.LoadControl(controlSrc);

        return Task.FromResult<Control?>(control);
    }
}
