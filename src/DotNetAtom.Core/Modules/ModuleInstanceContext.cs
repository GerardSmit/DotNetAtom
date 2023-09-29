using System;
using DotNetAtom.Entities;
using DotNetAtom.Entities.Portals;

namespace DotNetAtom.Modules;

public sealed class ModuleInstanceContext
{
    private readonly IModuleControl _moduleControl;
    private IModuleInfo? _configuration;
    private IPortalSettings? _portalSettings;

    public ModuleInstanceContext(IModuleControl moduleControl)
    {
        _moduleControl = moduleControl;
    }

    public IModuleInfo Configuration
    {
        get => _configuration ?? throw new InvalidOperationException("Configuration not set");
        set => _configuration = value;
    }

    public IPortalSettings PortalSettings
    {
        get => _portalSettings ?? throw new InvalidOperationException("Portal settings not set");
        set => _portalSettings = value;
    }
}
