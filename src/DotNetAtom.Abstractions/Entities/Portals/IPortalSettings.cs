using DotNetAtom.Portals;

namespace DotNetAtom.Entities.Portals;

public interface IPortalSettings
{
    ITabInfo? ActiveTab { get; set; }

    IPortalInfo Portal { get; set; }

    /// <summary>Gets or sets the portals logo file.</summary>
    string LogoFile { get; set; }

    string PortalName { get; set; }
}
