using System;
using System.Collections.Generic;
using DotNetAtom.Entities;
using DotNetAtom.UI.Skins;

namespace DotNetAtom.Tabs.Entities;

public class ActiveTabInfo : ITabInfo
{
    private ITabInfo _tabInfo = default!;
    private Dictionary<string, IPane>? _panes;

    public int TabId => _tabInfo.TabId;

    public int TabOrder => _tabInfo.TabOrder;

    public int? ParentId => _tabInfo.ParentId;

    public int? PortalId => _tabInfo.PortalId;

    public string? CultureCode => _tabInfo.CultureCode;

    public string TabName => _tabInfo.TabName;

    public string? Title => _tabInfo.Title;

    public string TabPath => _tabInfo.TabPath;

    public bool IsVisible => _tabInfo.IsVisible;

    public bool IsDeleted => _tabInfo.IsDeleted;

    public bool DisableLink => _tabInfo.DisableLink;

    public string? Description => _tabInfo.Description;

    public string? SkinSrc => _tabInfo.SkinSrc;

    public string? KeyWords => _tabInfo.KeyWords;

    public string? PageHeadText => _tabInfo.PageHeadText;

    public string? Url => _tabInfo.Url;

    public double SiteMapPriority => _tabInfo.SiteMapPriority;

    public Dictionary<string, IPane> Panes => _panes ??= new Dictionary<string, IPane>();

    public DateTime? CreatedOnDate => _tabInfo.CreatedOnDate;

    public DateTime? LastModifiedOnDate => _tabInfo.LastModifiedOnDate;

    public IEnumerable<IModuleInfo> TabModules => _tabInfo.TabModules;

    public IReadOnlyDictionary<string, string> TabSettings => _tabInfo.TabSettings;

    public IEnumerable<ITabPermissionInfo> Permissions => _tabInfo.Permissions;

    public void Initialize(ITabInfo tab)
    {
        _tabInfo = tab;
    }

    public void Clear()
    {
        _tabInfo = default!;
        _panes?.Clear();
    }
}
