using System.Threading.Tasks;
using DotNetAtom.Entities;
using DotNetAtom.Entities.Portals;
using WebFormsCore.UI;

namespace DotNetAtom.Modules;

public interface IModuleControlFactory
{
    string Extension { get; }

    Task<Control?> CreateControlAsync(Page page, IPortalSettings settings, IModuleInfo module, string? controlKey, string controlSrc);
}
