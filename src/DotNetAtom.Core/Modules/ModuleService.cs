using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetAtom.Entities;
using DotNetAtom.Memory;
using Microsoft.Extensions.Logging;

namespace DotNetAtom.Modules;

public class ModuleService : IModuleService
{
    private Dictionary<int, IModuleDefinitionInfo> _moduleDefinitions = new();
    private Dictionary<string, IModuleDefinitionInfo> _moduleDefinitionsByName = new();
    private ILogger<ModuleService> _logger;
    private readonly IModuleRepository _moduleRepository;

    public ModuleService(IModuleRepository moduleRepository, ILogger<ModuleService> logger)
    {
        _moduleRepository = moduleRepository;
        _logger = logger;
    }

    public IModuleDefinitionInfo GetDefinition(IModuleInfo info)
    {
        if (info.ModuleDefinitionId.HasValue)
        {
            return GetDefinition(info.ModuleDefinitionId.Value);
        }

        if (info.ModuleDefinitionFriendlyName is not null)
        {
            return GetDefinition(info.ModuleDefinitionFriendlyName);
        }

        if (info is InMemoryModuleInfo { ModuleDefinition: {} module })
        {
            return module;
        }

        throw new KeyNotFoundException($"No module definition found for module {info.ModuleId}.");
    }

    public IModuleDefinitionInfo GetDefinition(int moduleDefId)
    {
        return _moduleDefinitions.TryGetValue(moduleDefId, out var moduleDefinition)
            ? moduleDefinition
            : throw new KeyNotFoundException($"Module definition {moduleDefId} not found.");
    }

    public IModuleDefinitionInfo GetDefinition(string moduleName)
    {
        return _moduleDefinitionsByName.TryGetValue(moduleName, out var moduleDefinition)
            ? moduleDefinition
            : throw new KeyNotFoundException($"Module definition {moduleName} not found.");
    }

    public async Task LoadAsync()
    {
        var definitions = await _moduleRepository.GetDefinitionsAsync();

        _moduleDefinitions = definitions as Dictionary<int, IModuleDefinitionInfo> ?? definitions.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        _moduleDefinitionsByName = new Dictionary<string, IModuleDefinitionInfo>(_moduleDefinitions.Values.Count);

        foreach (var module in _moduleDefinitions.Values)
        {
            if (_moduleDefinitionsByName.ContainsKey(module.FriendlyName))
            {
                _logger.LogWarning("Module definition {FriendlyName} already exists.", module.FriendlyName);
                continue;
            }

            _moduleDefinitionsByName.Add(module.FriendlyName, module);
        }
    }
}
