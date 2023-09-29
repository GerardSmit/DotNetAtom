namespace DotNetAtom;

public interface IAtomGlobals
{
    /// <summary>
    /// Gets the application path.
    /// </summary>
    string ApplicationPath { get; }

    /// <summary>
    /// Gets the desktop module path.
    /// </summary>
    /// <value>
    /// ApplicationPath + "/DesktopModules/".
    /// </value>
    string DesktopModulePath { get; }

    /// <summary>
    /// Gets the image path.
    /// </summary>
    /// <value>
    /// ApplicationPath + "/Images/".
    /// </value>
    string ImagePath { get; }

    /// <summary>
    /// Gets the host path.
    /// </summary>
    /// <value>
    /// ApplicationPath + "/Portals/_default/".
    /// </value>
    string HostPath { get; }
}
