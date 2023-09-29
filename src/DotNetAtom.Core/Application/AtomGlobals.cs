using Microsoft.Extensions.Hosting;

namespace DotNetAtom.Application;

public class AtomGlobals : IAtomGlobals
{
    public AtomGlobals(IHostEnvironment webHostEnvironment)
    {
        ApplicationPath = webHostEnvironment.ContentRootPath;
        DesktopModulePath = $"{ApplicationPath}/DesktopModules/";
        ImagePath = $"{ApplicationPath}/Images/";
        HostPath = $"{ApplicationPath}/Portals/_default/";
    }

    public string ApplicationPath { get; }

    public string DesktopModulePath { get; }

    public string ImagePath { get; }

    public string HostPath { get; }
}
