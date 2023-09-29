using Dapper;

namespace DotNetAtom.Entities;

public class Role
{
    [DbValue(Name = "RoleID")]
    public int Id { get; set; }

    [DbValue(Name = "PortalID")]
    public int PortalId { get; set; }

    [DbValue(Name = "RoleName")]
    public string Name { get; set; }

    public Portal Portal { get; set; } = default!;

    public bool IsPublic { get; set; }

    public ICollection<TabPermission> TabPermissions { get; set; } = new List<TabPermission>();

    public ICollection<ModulePermission> ModulePermissions { get; set; } = new List<ModulePermission>();
}
