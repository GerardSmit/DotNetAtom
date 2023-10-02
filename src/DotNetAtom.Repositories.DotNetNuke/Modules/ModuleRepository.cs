using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetAtom.Entities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Definitions;

namespace DotNetAtom.Modules;

public class ModuleRepository : IModuleRepository
{
    private readonly IModuleController _controller;

    public ModuleRepository(IModuleController controller)
    {
        _controller = controller;
    }

    public Task<IReadOnlyDictionary<int, IModuleDefinitionInfo>> GetDefinitionsAsync()
    {
        return Task.FromResult<IReadOnlyDictionary<int, IModuleDefinitionInfo>>(
            _controller.GetAllModules()
                .Cast<ModuleDefinitionInfo>()
                .Select(def => new ModuleDefinitionInfoWrapper(def))
                .Cast<IModuleDefinitionInfo>()
                .ToDictionary(def => def.ModuleDefId)
        );
    }
}
