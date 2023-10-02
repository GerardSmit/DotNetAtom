using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DotNetAtom.Entities;
using DotNetAtom.Providers;
using DotNetAtom.UI.Skins;

namespace DotNetAtom;

// TODO: Remove this class.

/// <summary>
/// Temporary test tab provider.
/// </summary>
public class TestTabProvider : ITabProvider
{
    public ValueTask<IReadOnlyList<ITabInfo>> GetTabs()
    {
        return new ValueTask<IReadOnlyList<ITabInfo>>(
            new[]
            {
                new TabInfo
                {
                    TabName = "Account",
                    TabPath = "//Account",
                    IsVisible = true
                },
                new TabInfo
                {
                    TabName = "Login",
                    TabPath = "//Account//Login",
                    IsVisible = true,
                    TabModules =
                    {
                        new ModuleInfo
                        {
                            ModuleDefinitionFriendlyName = "Account Login"
                        }
                    }
                }
            }
        );
    }

    public class TabInfo : ITabInfo
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

    public class ModuleInfo : IModuleInfo
    {
        public int Id { get; set; }
        public int TabId { get; set; }
        public int? PortalId { get; set; }
        public string? ModuleTitle { get; set; }
        public int? ModuleId { get; set; }
        public string PaneName { get; set; } = "";
        public int ModuleOrder { get; set; }
        public int? ModuleDefinitionId { get; set; }
        public required string? ModuleDefinitionFriendlyName { get; set; }
        public string? HtmlContent { get; set; }
        public string? ContainerSrc { get; set; }
        public DateTime? LastHtmlModifiedOnDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? CreatedOnDate { get; set; }
        public DateTime? LastModifiedOnDate { get; set; }
        public bool InheritViewPermissions { get; set; }
        public IReadOnlyDictionary<string, string> Settings { get; set; } = new Dictionary<string, string>();
        public IEnumerable<IModulePermissionInfo> Permissions { get; set; } = new List<IModulePermissionInfo>();
    }
}
