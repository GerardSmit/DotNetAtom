using System.Collections.Generic;

namespace DotNetAtom.Entities;

public interface IModuleDefinitionInfo
{
    int ModuleDefId { get; }

    string FriendlyName { get; }

    IDesktopModuleInfo DesktopModule { get; }

    IReadOnlyDictionary<StringKey, IModuleControlInfo> Controls { get; }
}
