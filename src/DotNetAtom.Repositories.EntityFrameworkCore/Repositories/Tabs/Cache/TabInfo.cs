using System;
using System.Collections.Generic;
using System.Linq;
using DotNetAtom.Entities;
using DotNetAtom.UI.Skins;

namespace DotNetAtom.Tabs.Cache;

internal class TabInfo : ITabInfo
{
    public TabInfo(Tab tab)
    {
        TabId = tab.Id;
        TabOrder = tab.TabOrder;
        ParentId = tab.ParentId;
        PortalId = tab.PortalId;
        CultureCode = tab.CultureCode;
        TabName = tab.TabName;
        Title = tab.Title;
        TabPath = tab.TabPath;
        IsVisible = tab.IsVisible;
        IsDeleted = tab.IsDeleted;
        DisableLink = tab.DisableLink;
        Description = tab.Description;
        SkinSrc = tab.SkinSrc;
        ContainerSrc = tab.ContainerSrc;
        KeyWords = tab.KeyWords;
        PageHeadText = tab.PageHeadText;
        Url = tab.Url;
        SiteMapPriority = tab.SiteMapPriority;
        CreatedOnDate = tab.CreatedOnDate;
        LastModifiedOnDate = tab.LastModifiedOnDate;
        TabModules = tab.TabModules.Select(m => new ModuleInfo(m)).ToArray();
        TabSettings = tab.TabSettings.ToDictionary(k => k.SettingName, v => v.SettingValue);
        Permissions = tab.TabPermissions.Select(p => new TabPermissionInfo(p)).ToArray();
    }

    public int? TabId { get; }

    public int TabOrder { get; }

    public int? ParentId { get; }

    public int? PortalId { get; }

    public string? CultureCode { get; }

    public string TabName { get; }

    public string? Title { get; }

    public string TabPath { get; }

    public bool IsVisible { get; }

    public bool IsDeleted { get; }

    public bool IsAdmin => false;

    public bool DisableLink { get; }

    public string? Description { get; }

    public string? SkinSrc { get; }

    public string? ContainerSrc { get; set; }

    public string? KeyWords { get; }

    public string? PageHeadText { get; }

    public string? Url { get; }

    public double SiteMapPriority { get; }

    public Dictionary<string, IPane> Panes => throw new InvalidOperationException("Cannot access Panes on a cached TabInfo object");

    public DateTime? CreatedOnDate { get; }

    public DateTime? LastModifiedOnDate { get; }

    public IEnumerable<IModuleInfo> TabModules { get; }

    public IReadOnlyDictionary<string, string> TabSettings { get; }

    public IEnumerable<ITabPermissionInfo> Permissions { get; }
}
