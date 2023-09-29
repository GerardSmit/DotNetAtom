using System;
using DotNetAtom.Entities;

namespace DotNetAtom.Tabs.Cache;

public class DesktopModuleInfo : IDesktopModuleInfo
{
    public DesktopModuleInfo(DesktopModule desktopModule)
    {
        DesktopModuleId = desktopModule.Id;
        FriendlyName = desktopModule.FriendlyName;
        Description = desktopModule.Description;
        Version = desktopModule.Version;
        IsPremium = desktopModule.IsPremium;
        IsAdmin = desktopModule.IsAdmin;
        BusinessControllerClass = desktopModule.BusinessControllerClass;
        FolderName = desktopModule.FolderName;
        ModuleName = desktopModule.ModuleName;
        SupportedFeatures = desktopModule.SupportedFeatures;
        CompatibleVersions = desktopModule.CompatibleVersions;
        Dependencies = desktopModule.Dependencies;
        Permissions = desktopModule.Permissions;
        PackageId = desktopModule.PackageId;
        CreatedOnDate = desktopModule.CreatedOnDate;
        LastModifiedOnDate = desktopModule.LastModifiedOnDate;
    }

    public int DesktopModuleId { get; }

    public string FriendlyName { get; }

    public string? Description { get; }

    public string? Version { get; }

    public bool IsPremium { get; }

    public bool IsAdmin { get; }

    public string? BusinessControllerClass { get; }

    public string FolderName { get; }

    public string ModuleName { get; }

    public int SupportedFeatures { get; }

    public string? CompatibleVersions { get; }

    public string? Dependencies { get; }

    public string? Permissions { get; }

    public int PackageId { get; }

    public DateTime? CreatedOnDate { get; }

    public DateTime? LastModifiedOnDate { get; }
}