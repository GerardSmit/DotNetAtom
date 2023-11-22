using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetAtom.Collections;
using DotNetAtom.Entities;
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

public class ModuleInfoWrapper : IModuleInfo
{
    private readonly ModuleInfo _module;
    private HashtableDictionary<string, string>? _settings;

    public ModuleInfoWrapper(ModuleInfo module)
    {
        _module = module;
    }

    public int? Id
    {
        get => _module.ModuleID;
        set => _module.ModuleID = value ?? -1;
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