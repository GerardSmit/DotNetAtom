using Dapper;

namespace DotNetAtom.Entities;

public class PortalLocalization : ITimestamp
{
    [DbValue(Name = "PortalID")]
    public int PortalId { get; set; }

    public string CultureCode { get; set; }

    public string PortalName { get; set; }

    public string? LogoFile { get; set; }

    public string? FooterText { get; set; }

    public string? Description { get; set; }

    public string? KeyWords { get; set; }

    public string? BackgroundFile { get; set; }

    public int? HomeTabId { get; set; }

    public int? LoginTabId { get; set; }

    public int? UserTabId { get; set; }

    public int? AdminTabId { get; set; }

    public int? SplashTabId { get; set; }

    [DbValue(Name = "CreatedByUserID")]
    public int? CreatedByUserId { get; set; }

    public DateTime? CreatedOnDate { get; set; }

    [DbValue(Name = "LastModifiedByUserID")]
    public int? LastModifiedByUserId { get; set; }

    public DateTime? LastModifiedOnDate { get; set; }

    public int? RegisterTabId { get; set; }

    public int? SearchTabId { get; set; }
}
