﻿using System;
using Dapper;

namespace DotNetAtom.Entities;

public class ModulePermission : IEntity, ITimestamp
{
    [DbValue(Name = "ModulePermissionID")]
    public int Id { get; set; }

    [DbValue(Name = "ModuleID")]
    public int ModuleId { get; set; }

    [DbValue(Name = "PermissionID")]
    public int PermissionId { get; set; }
    public bool AllowAccess { get; set; }

    [DbValue(Name = "RoleID")]
    public int? RoleId { get; set; }

    [DbValue(Name = "UserID")]
    public int? UserId { get; set; }

    [DbValue(Name = "CreatedByUserID")]
    public int? CreatedByUserId { get; set; }

    public DateTime? CreatedOnDate { get; set; }

    [DbValue(Name = "LastModifiedByUserID")]
    public int? LastModifiedByUserId { get; set; }

    public DateTime? LastModifiedOnDate { get; set; }
}
