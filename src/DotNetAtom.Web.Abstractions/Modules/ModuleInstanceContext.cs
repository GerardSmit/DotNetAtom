using System;
using DotNetAtom.Entities;
using DotNetAtom.Entities.Portals;

namespace DotNetAtom.Modules;

public sealed class ModuleInstanceContext
{
    private IModuleInfo? _configuration;
    private IPortalSettings? _portalSettings;
    private IModuleHost? _moduleHost;

    public ModuleInstanceContext(IModuleControl moduleControl)
    {
        ModuleControl = moduleControl;
    }

    public IModuleControl ModuleControl { get; }

    public IModuleHost ModuleHost
    {
        get => _moduleHost ?? throw new InvalidOperationException("Module host not set");
        set => _moduleHost = value;
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
