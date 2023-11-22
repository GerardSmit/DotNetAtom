using System.Collections.Generic;
using DotNetAtom.Entities;

namespace DotNetAtom.Memory;

public class InMemoryModuleDefinition : IModuleDefinitionInfo
{
	public int ModuleDefId { get; set; }
	public string FriendlyName { get; set; } = "";
	public InMemoryDesktopModule DesktopModule { get; set; } = new();
	public Dictionary<StringKey, IModuleControlInfo> Controls { get; set; } = new();

	IDesktopModuleInfo IModuleDefinitionInfo.DesktopModule => DesktopModule;
	IReadOnlyDictionary<StringKey, IModuleControlInfo> IModuleDefinitionInfo.Controls => Controls;
}