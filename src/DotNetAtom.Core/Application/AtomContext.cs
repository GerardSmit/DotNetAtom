using System;
using DotNetAtom.Entities;
using DotNetAtom.Entities.Portals;
using DotNetAtom.Portals;

namespace DotNetAtom.Application;

internal sealed class AtomContext : IAtomContext
{
    private readonly PortalSettings _defaultPortalSettings;
    private Guid? _applicationId;
    private IPortalSettings? _portalSettings;

    internal AtomContextFactory? Factory;

    public AtomContext()
    {
        _defaultPortalSettings = new PortalSettings();
        _portalSettings = _defaultPortalSettings;
    }

    public Guid ApplicationId
    {
        get => _applicationId ?? throw new InvalidOperationException("Application not found");
        set => _applicationId = value;
    }

    public IPortalSettings PortalSettings
    {
        get => _portalSettings ?? throw new InvalidOperationException("Portal not found");
        set => _portalSettings = value;
    }

    public void Initialize(IPortalInfo portal, ITabInfo? tab)
    {
        _defaultPortalSettings.Initialize(portal, tab);
    }

    public void Clear()
    {
        _applicationId = null;
        _portalSettings = _defaultPortalSettings;
        _defaultPortalSettings.Clear();
    }

    public void Dispose()
    {
        Factory?.Return(this);
    }
}
