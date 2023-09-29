using Dapper;

namespace DotNetAtom.Entities;

public class PortalDesktopModule : IEntity
{
    [DbValue(Name = "PortalDesktopModuleID")]
    public int Id { get; set; }

    [DbValue(Name = "PortalID")]
    public int PortalId { get; set; }

    [DbValue(Name = "DesktopModuleID")]
    public int DesktopModuleId { get; set; }
}
