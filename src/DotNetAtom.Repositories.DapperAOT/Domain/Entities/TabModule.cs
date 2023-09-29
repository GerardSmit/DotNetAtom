using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using Dapper;

namespace DotNetAtom.Entities;

public class TabModule : ITimestamp
{
    /// <inheritdoc cref="IEntity.Id" />
    [DbValue(Name = "TabModuleID")]
    public int Id { get; set; }

    public int TabId { get; set; }

    public int? PortalId { get; set; }

    [DbValue(Name = "ModuleID")]
    public int ModuleId { get; set; }

    public string PaneName { get; set; }

    public int ModuleOrder { get; set; }

    public int CacheTime { get; set; }

    public string? Alignment { get; set; }

    public string? Color { get; set; }

    public string? Border { get; set; }

    public string? IconFile { get; set; }

    public int Visibility { get; set; }

    public string? ContainerSrc { get; set; }

    public DateTime? WebSliceExpiryDate { get; set; }

    [DbValue(Name = "WebSliceTTL")]
    public int? WebSliceTtl { get; set; }

    [DbValue(Name = "CreatedByUserID")]
    public int? CreatedByUserId { get; set; }

    public DateTime? CreatedOnDate { get; set; }

    [DbValue(Name = "LastModifiedByUserID")]
    public int? LastModifiedByUserId { get; set; }

    public DateTime? LastModifiedOnDate { get; set; }

    public bool IsDeleted { get; set; }

    public string? CacheMethod { get; set; }

    public string? ModuleTitle { get; set; }

    public string? Header { get; set; }

    public string? Footer { get; set; }

    public string? CultureCode { get; set; }

    public Guid UniqueId { get; set; }

    public Guid VersionGuid { get; set; }

    public Guid? DefaultLanguageGuid { get; set; }

    public Guid LocalizedVersionGuid { get; set; }
}
