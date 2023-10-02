using System;
using Dapper;

namespace DotNetAtom.Entities;

public class PortalAlias : IEntity, ITimestamp
{
    [DbValue(Name = "PortalAliasID")]
    public int Id { get; set; }

    [DbValue(Name = "PortalID")]
    public int PortalId { get; set; }

    [DbValue(Name = "HTTPAlias")]
    public string? HttpAlias { get; set; }

    [DbValue(Name = "CreatedByUserID")]
    public int? CreatedByUserId { get; set; }

    public DateTime? CreatedOnDate { get; set; }

    [DbValue(Name = "LastModifiedByUserID")]
    public int? LastModifiedByUserId { get; set; }

    public DateTime? LastModifiedOnDate { get; set; }
}
