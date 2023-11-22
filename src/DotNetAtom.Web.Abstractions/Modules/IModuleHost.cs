using DotNetAtom.Entities;

namespace DotNetAtom.Modules;

public interface IModuleHost
{
	IModuleInfo ModuleInfo { get; }

	IModuleControl ModuleControl { get; }
}
