using System.Threading.Tasks;
using DotNetAtom.Entities;
using DotNetAtom.Entities.Portals;
using DotNetAtom.Framework;
using WebFormsCore.UI;

namespace DotNetAtom.Modules;

public interface IModuleControlService
{
    Task<Control?> CreateModuleControlAsync(
        Page page,
        IPortalSettings settings,
        IModuleHost moduleHost,
        IModuleInfo module,
        string? controlKey);

    string GetModuleName(PortalModuleBase portalModuleBase);
}
