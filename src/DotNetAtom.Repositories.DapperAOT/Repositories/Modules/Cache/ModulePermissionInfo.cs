using DotNetAtom.Entities;

namespace DotNetAtom.Tabs.Cache;

internal class ModulePermissionInfo : IModulePermissionInfo
{
    public ModulePermissionInfo(ModulePermission modulePermission)
    {
        Id = modulePermission.Id;
        PermissionId = modulePermission.PermissionId;
        RoleId = modulePermission.RoleId;
        UserId = modulePermission.UserId;
        AllowAccess = modulePermission.AllowAccess;
    }

    public int Id { get; set; }
    public int PermissionId { get; }
    public int? RoleId { get; }
    public int? UserId { get; }
    public bool AllowAccess { get; }
}
