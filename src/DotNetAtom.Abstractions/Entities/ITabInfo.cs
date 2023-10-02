using System;
using System.Collections.Generic;
using DotNetAtom.UI.Skins;

namespace DotNetAtom.Entities;

public interface ITabInfo
{
    int? TabId { get; }

    int TabOrder { get; }

    int? ParentId { get; }

    int? PortalId { get; }

    string? CultureCode { get; }

    string TabName { get; }

    string? Title { get; }

    string TabPath { get; }

    bool IsVisible { get; }

    bool IsDeleted { get; }

    bool DisableLink { get; }

    string? Description { get; }

    string? SkinSrc { get; }

    string? KeyWords { get; }

    string? PageHeadText { get; }

    string? Url { get; }

    double SiteMapPriority { get; }

    Dictionary<string, IPane> Panes { get; }

    DateTime? CreatedOnDate { get; }

    DateTime? LastModifiedOnDate { get; }

    IEnumerable<IModuleInfo> TabModules { get; }

    IReadOnlyDictionary<string, string> TabSettings { get; }

    IEnumerable<ITabPermissionInfo> Permissions { get; }
}
