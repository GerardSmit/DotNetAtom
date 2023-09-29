using Dapper;

namespace DotNetAtom.Entities;

public class PortalSetting : ITimestamp
{
    [DbValue(Name = "PortalSettingID")]
    public int PortalSettingId { get; set; }

    public int PortalId { get; set; }

    public string SettingName { get; set; } = null!;

    public string SettingValue { get; set; } = null!;

    [DbValue(Name = "CreatedByUserID")]
    public int? CreatedByUserId { get; set; }

    public DateTime? CreatedOnDate { get; set; }

    [DbValue(Name = "LastModifiedByUserID")]
    public int? LastModifiedByUserId { get; set; }

    public DateTime? LastModifiedOnDate { get; set; }

    public string? CultureCode { get; set; }

    public bool IsSecure { get; set; }
}
