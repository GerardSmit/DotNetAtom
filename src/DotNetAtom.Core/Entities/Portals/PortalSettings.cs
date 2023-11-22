using System;
using DotNetAtom.Portals;
using DotNetAtom.Tabs.Entities;

namespace DotNetAtom.Entities.Portals;

public class PortalSettings : IPortalSettings
{
    private readonly ActiveTabInfo _activeTab = new();
    private IPortalInfo? _portal;

    public ITabInfo? ActiveTab { get; set; }

    public IPortalInfo Portal
    {
        get => _portal ?? throw new InvalidOperationException("There is no portal");
        set => _portal = value;
    }

    public IUserInfo User { get; set; } = AnonymousUserInfo.Instance;

    public string? LogoFile
    {
        get => Portal.LogoFile;
        set => Portal.LogoFile = value;
    }

    public string PortalName
    {
        get => Portal.PortalName;
        set => Portal.PortalName = value;
    }

    public string? CurrentSkinPath { get; set; }

    public string? CurrentSkinDirectory { get; set; }

    public void Initialize(IPortalInfo portal, ITabInfo? tab)
    {
        if (tab is not null)
        {
            _activeTab.Initialize(tab);
            ActiveTab = _activeTab;
        }

        _portal = portal;
    }

    public void Clear()
    {
        _activeTab.Clear();
        ActiveTab = null;
        _portal = null;
        CurrentSkinPath = null;
        CurrentSkinDirectory = null;
    }
}
