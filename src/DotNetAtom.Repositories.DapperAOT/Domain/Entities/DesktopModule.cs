using System;
using Dapper;

namespace DotNetAtom.Entities;

public class DesktopModule : IEntity, ITimestamp
{
    [DbValue(Name = "DesktopModuleID")]
    public int Id { get; set; }

    public string FriendlyName { get; set; }

    public string? Description { get; set; }

    public string? Version { get; set; }

    public bool IsPremium { get; set; }

    public bool IsAdmin { get; set; }

    public string? BusinessControllerClass { get; set; }

    public string FolderName { get; set; }

    public string ModuleName { get; set; }

    public int SupportedFeatures { get; set; }

    public string? CompatibleVersions { get; set; }

    public string? Dependencies { get; set; }

    public string? Permissions { get; set; }

    [DbValue(Name = "PackageID")]
    public int PackageId { get; set; }

    [DbValue(Name = "CreatedByUserID")]
    public int? CreatedByUserId { get; set; }

    public DateTime? CreatedOnDate { get; set; }

    [DbValue(Name = "LastModifiedByUserID")]
    public int? LastModifiedByUserId { get; set; }

    public DateTime? LastModifiedOnDate { get; set; }

    public int ContentItemId { get; set; }
}

