using System;
using System.Collections.Generic;
using System.Linq;
using DotNetAtom.Entities;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Definitions;

namespace DotNetAtom.Modules;

internal class ModuleDefinitionInfoWrapper : IModuleDefinitionInfo
{
    private readonly ModuleDefinitionInfo _moduleDefinition;

    public ModuleDefinitionInfoWrapper(ModuleDefinitionInfo moduleDefinition)
    {
        _moduleDefinition = moduleDefinition;
        DesktopModule = new DesktopModuleInfoWrapper(DesktopModuleController.GetDesktopModule(moduleDefinition.DesktopModuleID, Null.NullInteger));
        Controls = moduleDefinition.ModuleControls.ToDictionary(
            control => new StringKey(control.Key),
            control => (IModuleControlInfo)new ModuleControlInfoWrapper(control.Value)
        );
    }

    public int ModuleDefId => _moduleDefinition.ModuleDefID;

    public IDesktopModuleInfo DesktopModule { get; }

    public IReadOnlyDictionary<StringKey, IModuleControlInfo> Controls { get; }
}

internal class ModuleControlInfoWrapper : IModuleControlInfo
{
    private readonly ModuleControlInfo _control;

    public ModuleControlInfoWrapper(ModuleControlInfo control)
    {
        _control = control;
    }

    public int ModuleControlId => _control.ModuleControlID;

    public string? ControlKey => _control.ControlKey;

    public string? ControlTitle => _control.ControlTitle;

    public string? ControlSrc => _control.ControlSrc;

    public string? IconFile => _control.IconFile;

    public string? HelpUrl => _control.HelpURL;

    public bool SupportsPartialRendering => _control.SupportsPartialRendering;

    public bool SupportsPopUps => _control.SupportsPopUps;

    public DateTime? CreatedOnDate => _control.CreatedOnDate;

    public DateTime? LastModifiedOnDate => _control.LastModifiedOnDate;
}

internal class DesktopModuleInfoWrapper : IDesktopModuleInfo
{
    private readonly DesktopModuleInfo _desktopModule;

    public DesktopModuleInfoWrapper(DesktopModuleInfo desktopModule)
    {
        _desktopModule = desktopModule;
    }

    public int DesktopModuleId => _desktopModule.DesktopModuleID;

    public string FriendlyName => _desktopModule.FriendlyName;

    public string? Description => _desktopModule.Description;

    public string? Version => _desktopModule.Version;

    public bool IsPremium => _desktopModule.IsPremium;

    public bool IsAdmin => _desktopModule.IsAdmin;

    public string? BusinessControllerClass => _desktopModule.BusinessControllerClass;

    public string FolderName => _desktopModule.FolderName;

    public string ModuleName => _desktopModule.ModuleName;

    public int SupportedFeatures => _desktopModule.SupportedFeatures;

    public string? CompatibleVersions => _desktopModule.CompatibleVersions;

    public string? Dependencies => _desktopModule.Dependencies;

    public string? Permissions => _desktopModule.Permissions;

    public int PackageId => _desktopModule.PackageID;

    public DateTime? CreatedOnDate => _desktopModule.CreatedOnDate;

    public DateTime? LastModifiedOnDate => _desktopModule.LastModifiedOnDate;
}
