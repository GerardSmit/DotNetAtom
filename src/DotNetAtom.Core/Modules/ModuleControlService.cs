using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DotNetAtom.Entities;
using DotNetAtom.Entities.Portals;
using WebFormsCore.UI;

namespace DotNetAtom.Modules;

public class ModuleControlService : IModuleControlService
{
    private readonly IModuleService _moduleService;
    private readonly Dictionary<string, IModuleControlFactory> _factories;

    public ModuleControlService(IModuleService moduleService, IEnumerable<IModuleControlFactory> factories)
    {
        _moduleService = moduleService;

        _factories = new Dictionary<string, IModuleControlFactory>(StringComparer.OrdinalIgnoreCase);

        foreach (var factory in factories)
        {
            var extension = factory.Extension;

            // ReSharper disable once CanSimplifyDictionaryLookupWithTryAdd
            if (!_factories.ContainsKey(extension))
            {
                _factories.Add(extension, factory);
            }
        }
    }

    public async Task<Control?> CreateModuleControlAsync(Page page, IPortalSettings settings, IModuleInfo module, string? controlKey)
    {
        var definition = _moduleService.GetDefinition(module.ModuleDefinitionId);

        if (!definition.Controls.TryGetValue(controlKey, out var controlDefinition) ||
            controlDefinition.ControlSrc is null)
        {
            return null;
        }

        var extension = Path.GetExtension(controlDefinition.ControlSrc);

        if (!_factories.TryGetValue(extension, out var factory))
        {
            return null;
        }

        var control = await factory.CreateControlAsync(page, settings, module, controlKey, controlDefinition.ControlSrc);

        if (control is IModuleControl moduleControl)
        {
            moduleControl.ModuleContext.Configuration = module;
            moduleControl.ModuleContext.PortalSettings = settings;
        }

        return control;
    }
}
