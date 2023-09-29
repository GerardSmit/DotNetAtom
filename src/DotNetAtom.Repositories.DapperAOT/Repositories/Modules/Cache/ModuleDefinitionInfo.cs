using System.Collections.Generic;
using System.Linq;
using DotNetAtom.Entities;

namespace DotNetAtom.Tabs.Cache;

public class ModuleDefinitionInfo : IModuleDefinitionInfo
{
    public ModuleDefinitionInfo(ModuleDefinition moduleDefinition, DesktopModule desktopModule, IEnumerable<ModuleControl> controls)
    {
        ModuleDefId = moduleDefinition.Id;
        DesktopModule = new DesktopModuleInfo(desktopModule);
        Controls = controls
            .ToDictionary(
                mc => (StringKey)mc.ControlKey,
                mc => (IModuleControlInfo)new ModuleControlInfo(mc));
    }

    public int ModuleDefId { get; }

    public IDesktopModuleInfo DesktopModule { get; }

    public IReadOnlyDictionary<StringKey, IModuleControlInfo> Controls { get; }
}
