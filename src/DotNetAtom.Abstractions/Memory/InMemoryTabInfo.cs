using System;
using System.Collections.Generic;
using DotNetAtom.Entities;
using DotNetAtom.UI.Skins;

namespace DotNetAtom.Memory;

public class InMemoryTabInfo : ITabInfo
{
    public int? TabId { get; set; }
    public int TabOrder { get; set; }
    public int? ParentId { get; set; }
    public int? PortalId { get; set; }
    public string? CultureCode { get; set; }
    public required string TabName { get; set; }
    public string? Title { get; set; }
    public required string TabPath { get; set; }
    public bool IsVisible { get; set; }
    public bool IsDeleted { get; set; }
    public bool DisableLink { get; set; }
    public string? Description { get; set; }
    public string? SkinSrc { get; set; }
    public string? KeyWords { get; set; }
    public string? PageHeadText { get; set; }
    public string? Url { get; set; }
    public double SiteMapPriority { get; set; }
    public Dictionary<string, IPane> Panes { get; set; } = new();
    public DateTime? CreatedOnDate { get; set; }
    public DateTime? LastModifiedOnDate { get; set; }
    public List<IModuleInfo> TabModules { get; set; } = new();
    public Dictionary<string, string> TabSettings { get; set; } = new();
    public List<ITabPermissionInfo> Permissions { get; set; } = new();

    IEnumerable<IModuleInfo> ITabInfo.TabModules => TabModules;
    IEnumerable<ITabPermissionInfo> ITabInfo.Permissions => Permissions;
    IReadOnlyDictionary<string, string> ITabInfo.TabSettings => TabSettings;
}