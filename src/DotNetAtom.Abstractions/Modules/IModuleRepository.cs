using System.Collections.Generic;
using System.Threading.Tasks;
using DotNetAtom.Entities;

namespace DotNetAtom.Modules;

public interface IModuleRepository
{
    Task<IReadOnlyDictionary<int, IModuleDefinitionInfo>> GetDefinitionsAsync();
}
