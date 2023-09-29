using System.Collections.Generic;
using System.Threading.Tasks;
using DotNetAtom.Entities;
using WebFormsCore;

namespace DotNetAtom.Modules;

public class ModuleService : IModuleService
{
    private IReadOnlyDictionary<int, IModuleDefinitionInfo> _moduleDefinitions = new Dictionary<int, IModuleDefinitionInfo>();
    private readonly IModuleRepository _moduleRepository;

    public ModuleService(IModuleRepository moduleRepository)
    {
        _moduleRepository = moduleRepository;
    }

    public async Task LoadAsync()
    {
        _moduleDefinitions = await _moduleRepository.GetDefinitionsAsync();
    }

    public IModuleDefinitionInfo GetDefinition(int moduleDefId)
    {
        return _moduleDefinitions.TryGetValue(moduleDefId, out var moduleDefinition)
            ? moduleDefinition
            : throw new KeyNotFoundException($"Module definition {moduleDefId} not found.");
    }
}
