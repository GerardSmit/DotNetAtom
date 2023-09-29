using Dapper;

namespace DotNetAtom.Entities;

public class Package : IEntity, ITimestamp
{
    [DbValue(Name = "PackageID")]
    public int Id { get; set; }

    [DbValue(Name = "PortalID")]
    public int? PortalId { get; set; }

    public string Name { get; set; }

    public string FriendlyName { get; set; }

    public string? Description { get; set; }

    public required string PackageType { get; set; }

    public required string Version { get; set; }

    public string? License { get; set; }

    public string? Owner { get; set; }

    public string? Organization { get; set; }

    public string? Url { get; set; }

    public string? Email { get; set; }

    public string? ReleaseNotes { get; set; }

    public bool IsSystemPackage { get; set; }

    [DbValue(Name = "CreatedByUserID")]
    public int? CreatedByUserId { get; set; }

    public DateTime? CreatedOnDate { get; set; }

    [DbValue(Name = "LastModifiedByUserID")]
    public int? LastModifiedByUserId { get; set; }

    public DateTime? LastModifiedOnDate { get; set; }

    public string? FolderName { get; set; }

    public string? IconFile { get; set; }
}
