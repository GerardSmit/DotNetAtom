using System.Threading.Tasks;
using DotNetAtom.Entities;

namespace DotNetAtom.Modules;

public interface IModuleService
{
    IModuleDefinitionInfo GetDefinition(IModuleInfo info);

    IModuleDefinitionInfo GetDefinition(int moduleDefId);

    IModuleDefinitionInfo GetDefinition(string moduleName);

    Task LoadAsync();
}
