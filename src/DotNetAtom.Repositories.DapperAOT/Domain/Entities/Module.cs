using System;
using Dapper;

namespace DotNetAtom.Entities;

public class Module : IEntity, ITimestamp
{
    /// <inheritdoc />
    [DbValue(Name = "ModuleID")]
    public int Id { get; set; }

    [DbValue(Name = "ModuleDefID")]
    public int ModuleDefId { get; set; }

    public bool AllTabs { get; set; }

    public bool IsDeleted { get; set; }

    public bool? InheritViewPermissions { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    [DbValue(Name = "PortalID")]
    public int? PortalId { get; set; }

    [DbValue(Name = "CreatedByUserID")]
    public int? CreatedByUserId { get; set; }

    public DateTime? CreatedOnDate { get; set; }

    [DbValue(Name = "LastModifiedByUserID")]
    public int? LastModifiedByUserId { get; set; }

    public DateTime? LastModifiedOnDate { get; set; }

    public DateTime? LastContentModifiedOnDate { get; set; }

    [DbValue(Name = "ContentItemID")]
    public int? ContentItemId { get; set; }
}
