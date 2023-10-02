using System;
using System.Collections.Generic;
using System.Linq;
using DotNetAtom.Entities;

namespace DotNetAtom.Tabs.Cache;

internal class ModuleInfo : IModuleInfo
{
    public ModuleInfo(TabModule tabModule)
    {
        Id = tabModule.Id;
        TabId = tabModule.TabId;
        PortalId = tabModule.Tab.PortalId;
        PaneName = tabModule.PaneName;
        ModuleId = tabModule.ModuleId;
        ModuleOrder = tabModule.ModuleOrder;
        ModuleDefinitionId = tabModule.Module.ModuleDefId;
        HtmlContent = tabModule.HtmlContent;
        ContainerSrc = tabModule.ContainerSrc;
        LastHtmlModifiedOnDate = tabModule.LastHtmlModifiedOnDate;
        IsDeleted = tabModule.IsDeleted;
        CreatedOnDate = tabModule.CreatedOnDate;
        LastModifiedOnDate = tabModule.LastModifiedOnDate;
        Settings = tabModule.ModuleSettings.ToDictionary(kv => kv.SettingName, kv => kv.SettingValue);
        Permissions = tabModule.Module.Permissions.Select(m => new ModulePermissionInfo(m)).ToArray();
        InheritViewPermissions = tabModule.Module.InheritViewPermissions ?? false;
        ModuleTitle = tabModule.ModuleTitle;
    }

    public int Id { get; set; }
    public int TabId { get; }
    public int? PortalId { get; }
    public string? ModuleTitle { get; }
    public int? ModuleId { get; }
    public string PaneName { get; }
    public int ModuleOrder { get; }
    public int? ModuleDefinitionId { get; }
    public string? HtmlContent { get; }
    public string? ContainerSrc { get; }
    public DateTime? LastHtmlModifiedOnDate { get; }
    public bool IsDeleted { get; }
    public DateTime? CreatedOnDate { get; }
    public DateTime? LastModifiedOnDate { get; }
    public bool InheritViewPermissions { get; }
    public IReadOnlyDictionary<string, string> Settings { get; }
    public IEnumerable<IModulePermissionInfo> Permissions { get; }
    string? IModuleInfo.ModuleDefinitionFriendlyName => null;
}
