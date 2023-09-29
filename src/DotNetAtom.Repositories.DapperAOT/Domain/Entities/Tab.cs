using System.ComponentModel.DataAnnotations.Schema;
using Dapper;

namespace DotNetAtom.Entities;

public class Tab : ITimestamp
{
    [DbValue(Name = "TabID")]
    public int Id { get; set; }

    public int TabOrder { get; set; }

    [DbValue(Name = "PortalID")]
    public int? PortalId { get; set; }

    public Portal? Portal { get; set; }

    public string TabName { get; set; }

    public bool IsVisible { get; set; }

    public int? ParentId { get; set; }

    public string? IconFile { get; set; }

    public bool DisableLink { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? KeyWords { get; set; }

    public bool IsDeleted { get; set; }

    public string? Url { get; set; }

    public string? SkinSrc { get; set; }

    public string? ContainerSrc { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public int? RefreshInterval { get; set; }

    public string? PageHeadText { get; set; }

    public bool IsSecure { get; set; }

    public bool PermanentRedirect { get; set; }

    public double SiteMapPriority { get; set; }

    [DbValue(Name = "CreatedByUserID")]
    public int? CreatedByUserId { get; set; }

    public DateTime? CreatedOnDate { get; set; }

    [DbValue(Name = "LastModifiedByUserID")]
    public int? LastModifiedByUserId { get; set; }

    public DateTime? LastModifiedOnDate { get; set; }

    public string? IconFileLarge { get; set; }

    public string? CultureCode { get; set; }

    public int? ContentItemId { get; set; }

    public Guid UniqueId { get; set; }

    public Guid VersionGuid { get; set; }

    public Guid? DefaultLanguageGuid { get; set; }

    public Guid LocalizedVersionGuid { get; set; }

    public int Level { get; set; }

    public string TabPath { get; set; }
}
