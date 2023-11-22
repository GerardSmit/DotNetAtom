using System;
using System.Collections.Generic;
using DotNetAtom.Entities;

namespace DotNetAtom.Memory;

public class InMemoryModuleInfo : IModuleInfo
{
    public int? Id { get; set; }
    public int TabId { get; set; }
    public int? PortalId { get; set; }
    public string? ModuleTitle { get; set; }
    public int? ModuleId { get; set; }
    public string PaneName { get; set; } = "";
    public int ModuleOrder { get; set; }
    public int? ModuleDefinitionId { get; set; }
    public string? ModuleDefinitionFriendlyName { get; set; }
    public IModuleDefinitionInfo? ModuleDefinition { get; set; }
    public string? HtmlContent { get; set; }
    public string? ContainerSrc { get; set; }
    public DateTime? LastHtmlModifiedOnDate { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? CreatedOnDate { get; set; }
    public DateTime? LastModifiedOnDate { get; set; }
    public bool InheritViewPermissions { get; set; }
    public Dictionary<string, string> Settings { get; set; } = new();
    public List<IModulePermissionInfo> Permissions { get; set; } = new();

    IEnumerable<IModulePermissionInfo> IModuleInfo.Permissions => Permissions;
    IReadOnlyDictionary<string, string> IModuleInfo.Settings => Settings;
}
