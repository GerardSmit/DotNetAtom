using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetAtom.Collections;
using DotNetAtom.Entities;
using DotNetAtom.UI.Skins;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Tabs;

namespace DotNetAtom.Tabs;

public class TabRepository : ITabRepository
{
    private readonly IPortalController _portalController;
    private readonly ITabController _tabController;

    public TabRepository(ITabController tabController, IPortalController portalController)
    {
        _tabController = tabController;
        _portalController = portalController;
    }

    public Task<IReadOnlyList<ITabInfo>> GetTabsAsync()
    {
        var portalIds = _portalController.GetPortals()
            .Cast<DotNetNuke.Abstractions.Portals.IPortalInfo>()
            .Select(portal => portal.PortalId)
            .Distinct()
            .ToArray();

        var tabs = portalIds
            .SelectMany(portalId => _tabController.GetTabsByPortal(portalId))
            .Select(kv => kv.Value)
            .Select(tab => new TabInfoWrapper(tab))
            .Cast<ITabInfo>()
            .ToArray();

        return Task.FromResult<IReadOnlyList<ITabInfo>>(tabs);
    }
}

public class TabInfoWrapper : ITabInfo
{
    private readonly TabInfo _tab;
    private HashtableDictionary<string, string>? _settings;

    public TabInfoWrapper(TabInfo tab)
    {
        _tab = tab;
        TabModules = _tab.Modules.Cast<ModuleInfo>()
            .Select(module => new ModuleInfoWrapper(module))
            .Cast<IModuleInfo>()
            .ToArray();
    }

    public int? TabId => _tab.TabID;

    public int TabOrder => _tab.TabOrder;

    public int? ParentId => _tab.ParentId;

    public int? PortalId => _tab.PortalID;

    public string? CultureCode => _tab.CultureCode;

    public string TabName => _tab.TabName;

    public string? Title => _tab.Title;

    public string TabPath => _tab.TabPath;

    public bool IsVisible => _tab.IsVisible;

    public bool IsDeleted => _tab.IsDeleted;

    public bool DisableLink => _tab.DisableLink;

    public string? Description => _tab.Description;

    public string? SkinSrc => _tab.SkinSrc;

    public string? KeyWords => _tab.KeyWords;

    public string? PageHeadText => _tab.PageHeadText;

    public string? Url => _tab.Url;

    public double SiteMapPriority => _tab.SiteMapPriority;

    public Dictionary<string, IPane> Panes => throw new NotSupportedException();

    public DateTime? CreatedOnDate => _tab.CreatedOnDate;

    public DateTime? LastModifiedOnDate => _tab.LastModifiedOnDate;

    public IEnumerable<IModuleInfo> TabModules { get; }

    public IReadOnlyDictionary<string, string> TabSettings => _settings ??= new HashtableDictionary<string, string>(_tab.TabSettings, string.Empty);

    public IEnumerable<ITabPermissionInfo> Permissions => Enumerable.Empty<ITabPermissionInfo>();
}

public class ModuleInfoWrapper : IModuleInfo
{
    private readonly ModuleInfo _module;
    private HashtableDictionary<string, string>? _settings;

    public ModuleInfoWrapper(ModuleInfo module)
    {
        _module = module;
    }

    public int Id
    {
        get => _module.ModuleID;
        set => _module.ModuleID = value;
    }

    public int TabId => _module.TabID;

    public int? PortalId => _module.PortalID;

    public string? ModuleTitle => _module.ModuleTitle;

    public int? ModuleId => _module.ModuleID;

    public string PaneName => _module.PaneName;

    public int ModuleOrder => _module.ModuleOrder;

    public string ModuleFolder => _module.DesktopModule.FolderName;

    public string ModuleName => _module.DesktopModule.ModuleName;

    public int? ModuleDefinitionId => _module.ModuleDefID;

    public string? ModuleDefinitionFriendlyName => _module.ModuleDefinition.FriendlyName;

    public string? HtmlContent => _module.Content;

    public string? ContainerSrc => _module.ContainerSrc;

    public DateTime? LastHtmlModifiedOnDate => _module.LastContentModifiedOnDate;

    public bool IsDeleted => _module.IsDeleted;

    public DateTime? CreatedOnDate => _module.CreatedOnDate;

    public DateTime? LastModifiedOnDate => _module.LastModifiedOnDate;

    public bool InheritViewPermissions => _module.InheritViewPermissions;

    public IReadOnlyDictionary<string, string> Settings => _settings ??= new HashtableDictionary<string, string>(_module.ModuleSettings, string.Empty);

    public IEnumerable<IModulePermissionInfo> Permissions => Array.Empty<IModulePermissionInfo>();
}