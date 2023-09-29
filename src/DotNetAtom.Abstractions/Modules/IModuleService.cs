using System.Threading.Tasks;
using DotNetAtom.Entities;

namespace DotNetAtom.Modules;

public interface IModuleService
{
    IModuleDefinitionInfo GetDefinition(int moduleDefId);

    Task LoadAsync();
}
