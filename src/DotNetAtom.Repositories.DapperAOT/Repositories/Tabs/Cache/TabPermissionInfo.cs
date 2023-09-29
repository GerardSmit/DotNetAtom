using DotNetAtom.Entities;

namespace DotNetAtom.Tabs.Cache;

internal class TabPermissionInfo : ITabPermissionInfo
{
    public TabPermissionInfo(TabPermission tabPermission)
    {
        Id = tabPermission.Id;
        PermissionId = tabPermission.PermissionId;
        RoleId = tabPermission.RoleId;
        UserId = tabPermission.UserId;
        AllowAccess = tabPermission.AllowAccess;
    }

    public int Id { get; set; }
    public int PermissionId { get; }
    public int? RoleId { get; }
    public int? UserId { get; }
    public bool AllowAccess { get; }
}
