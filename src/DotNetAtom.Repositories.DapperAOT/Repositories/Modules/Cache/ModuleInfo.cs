using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using DotNetAtom.Entities;

namespace DotNetAtom.Tabs.Cache;

internal class ModuleInfo : IModuleInfo
{
    public ModuleInfo(
        TabModule tabModule,
        Module module,
        IEnumerable<HtmlText> texts,
        IEnumerable<TabModuleSetting> settings,
        IEnumerable<ModulePermission> permissions)
    {
        var lastText = texts.OrderByDescending(m => m.Version).FirstOrDefault();

        Id = tabModule.Id;
        TabId = tabModule.TabId;
        PortalId = tabModule.PortalId;
        PaneName = tabModule.PaneName;
        ModuleId = tabModule.ModuleId;
        ModuleOrder = tabModule.ModuleOrder;
        ModuleDefinitionId = module.ModuleDefId;
        HtmlContent = WebUtility.HtmlDecode(lastText?.Content);
        ContainerSrc = tabModule.ContainerSrc;
        LastHtmlModifiedOnDate = lastText?.LastModifiedOnDate;
        IsDeleted = tabModule.IsDeleted;
        CreatedOnDate = tabModule.CreatedOnDate;
        LastModifiedOnDate = tabModule.LastModifiedOnDate;
        Settings = settings.ToDictionary(kv => kv.SettingName, kv => kv.SettingValue);
        Permissions = permissions.Select(m => new ModulePermissionInfo(m)).ToArray();
        InheritViewPermissions = module.InheritViewPermissions ?? false;
        ModuleTitle = tabModule.ModuleTitle;
    }

    public int Id { get; set; }
    public int TabId { get; }
    public int? PortalId { get; }
    public string? ModuleTitle { get; }
    public int ModuleId { get; }
    public string PaneName { get; }
    public int ModuleOrder { get; }
    public int ModuleDefinitionId { get; }
    public string? HtmlContent { get; }
    public string? ContainerSrc { get; }
    public DateTime? LastHtmlModifiedOnDate { get; }
    public bool IsDeleted { get; }
    public DateTime? CreatedOnDate { get; }
    public DateTime? LastModifiedOnDate { get; }
    public bool InheritViewPermissions { get; }
    public IReadOnlyDictionary<string, string> Settings { get; }
    public IEnumerable<IModulePermissionInfo> Permissions { get; }
}
