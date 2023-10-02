using System.Collections.Generic;
using System.Linq;
using DotNetAtom.Entities;

namespace DotNetAtom.Tabs.Cache;

public class ModuleDefinitionInfo : IModuleDefinitionInfo
{
    public ModuleDefinitionInfo(ModuleDefinition moduleDefinition)
    {
        ModuleDefId = moduleDefinition.Id;
        FriendlyName = moduleDefinition.FriendlyName;
        DesktopModule = new DesktopModuleInfo(moduleDefinition.DesktopModule);
        Controls = moduleDefinition.ModuleControls
            .ToDictionary(
                mc => (StringKey)mc.ControlKey,
                mc => (IModuleControlInfo)new ModuleControlInfo(mc));
    }

    public int ModuleDefId { get; }

    public string FriendlyName { get; }

    public IDesktopModuleInfo DesktopModule { get; }

    public IReadOnlyDictionary<StringKey, IModuleControlInfo> Controls { get; }
}
