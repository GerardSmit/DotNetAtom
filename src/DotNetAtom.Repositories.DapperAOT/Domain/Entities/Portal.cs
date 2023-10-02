using System;
using Dapper;

namespace DotNetAtom.Entities;

public class Portal : IEntity, ITimestamp
{
    [DbValue(Name = "PortalID")]
    public int Id { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public int UserRegistration { get; set; }

    public int BannerAdvertising { get; set; }

    public int? AdministratorId { get; set; }

    public string? Currency { get; set; }

    public decimal HostFee { get; set; }

    public int HostSpace { get; set; }

    public int? AdministratorRoleId { get; set; }

    public int? RegisteredRoleId { get; set; }

    [DbValue(Name = "GUID")]
    public Guid Guid { get; set; }

    public string? PaymentProcessor { get; set; }

    public string? ProcessorUserId { get; set; }

    public string? ProcessorPassword { get; set; }

    public int? SiteLogHistory { get; set; }

    public string DefaultLanguage { get; set; }

    public int TimezoneOffset { get; set; }

    public string HomeDirectory { get; set; }

    public int PageQuota { get; set; }

    public int UserQuota { get; set; }

    [DbValue(Name = "CreatedByUserID")]
    public int? CreatedByUserId { get; set; }

    public DateTime? CreatedOnDate { get; set; }

    [DbValue(Name = "LastModifiedByUserID")]
    public int? LastModifiedByUserId { get; set; }

    public DateTime? LastModifiedOnDate { get; set; }

    [DbValue(Name = "PortalGroupID")]
    public int? PortalGroupId { get; set; }
}
