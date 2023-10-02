using System;
using System.Collections.Generic;

namespace DotNetAtom.Entities;

public interface IModuleInfo : IEntity
{
    int TabId { get; }

    int? PortalId { get; }

    string? ModuleTitle { get; }

    int? ModuleId { get; }

    string PaneName { get; }

    int ModuleOrder { get; }

    int? ModuleDefinitionId { get; }

    string? ModuleDefinitionFriendlyName { get; }

    string? HtmlContent { get; }

    string? ContainerSrc { get; }

    DateTime? LastHtmlModifiedOnDate { get; }

    bool IsDeleted { get; }

    DateTime? CreatedOnDate { get; }

    DateTime? LastModifiedOnDate { get; }

    bool InheritViewPermissions { get; }

    IReadOnlyDictionary<string, string> Settings { get; }

    IEnumerable<IModulePermissionInfo> Permissions { get; }
}
