using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Ben.Collections.Specialized;
using DotNetAtom.Entities;
using DotNetAtom.Entities.Portals;
using DotNetAtom.Framework;
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
        var definition = _moduleService.GetDefinition(module);

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

    public string GetModuleName(PortalModuleBase portalModuleBase)
    {
        var name = portalModuleBase.GetType().Name;

#if NET
        if (!name.Contains('.'))
        {
            return name;
        }
#else
        if (!name.Contains("."))
        {
            return name;
        }
#endif

        Span<char> span = stackalloc char[name.Length];

        for (var i = 0; i < name.Length; i++)
        {
            var c = name[i];
            span[i] = c == '.' ? '_' : c;
        }

        return InternPool.Shared.Intern(span);
    }
}
