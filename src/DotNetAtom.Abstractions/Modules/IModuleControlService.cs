using System.Threading.Tasks;
using DotNetAtom.Entities;
using DotNetAtom.Entities.Portals;
using WebFormsCore.UI;

namespace DotNetAtom.Modules;

public interface IModuleControlService
{
    Task<Control?> CreateModuleControlAsync(
        Page page,
        IPortalSettings settings,
        IModuleInfo module,
        string? controlKey);
}
