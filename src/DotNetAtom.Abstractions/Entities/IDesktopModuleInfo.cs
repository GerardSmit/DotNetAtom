using System;

namespace DotNetAtom.Entities;

public interface IDesktopModuleInfo
{
    int DesktopModuleId { get; }

    string FriendlyName { get; }

    string? Description { get; }

    string? Version { get; }

    bool IsPremium { get; }

    bool IsAdmin { get; }

    string? BusinessControllerClass { get; }

    string FolderName { get; }

    string ModuleName { get; }

    int SupportedFeatures { get; }

    string? CompatibleVersions { get; }

    string? Dependencies { get; }

    string? Permissions { get; }

    int PackageId { get; }

    DateTime? CreatedOnDate { get; }

    DateTime? LastModifiedOnDate { get; }
}
