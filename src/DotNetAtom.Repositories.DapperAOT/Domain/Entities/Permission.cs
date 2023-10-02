using System;
using System.Collections.Generic;
using Dapper;

namespace DotNetAtom.Entities;

public class Permission : IEntity, ITimestamp
{
    [DbValue(Name = "PermissionID")]
    public int Id { get; set; }

    public string PermissionCode { get; set; }

    [DbValue(Name = "ModuleDefID")]
    public int ModuleDefinitionId { get; set; }
        
    public string PermissionKey { get; set; }
        
    public string PermissionName { get; set; }
        
    public int ViewOrder { get; set; }

    [DbValue(Name = "CreatedByUserID")]
    public int? CreatedByUserId { get; set; }

    public DateTime? CreatedOnDate { get; set; }

    [DbValue(Name = "LastModifiedByUserID")]
    public int? LastModifiedByUserId { get; set; }

    public DateTime? LastModifiedOnDate { get; set; }
        
    public ICollection<ModulePermission> ModulePermissions { get; set; } = new List<ModulePermission>();
        
    public ICollection<TabPermission> TabPermissions { get; set; } = new List<TabPermission>();
}
