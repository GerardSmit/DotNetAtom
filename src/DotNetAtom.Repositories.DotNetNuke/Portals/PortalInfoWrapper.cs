using System;
using System.Collections.Generic;

namespace DotNetAtom.Portals;

public class PortalInfoWrapper : IPortalInfo
{
    private readonly DotNetNuke.Abstractions.Portals.IPortalInfo _portalInfo;

    public PortalInfoWrapper(DotNetNuke.Abstractions.Portals.IPortalInfo portalInfo, IReadOnlyDictionary<string, string> settings)
    {
        _portalInfo = portalInfo;
        Settings = settings;
    }

    public string? FooterText
    {
        get => _portalInfo.FooterText;
        set => _portalInfo.FooterText = value;
    }

    public string HomeDirectory
    {
        get => _portalInfo.HomeDirectory;
        set => _portalInfo.HomeDirectory = value;
    }

    public string HomeSystemDirectory => _portalInfo.HomeSystemDirectory;

    public string HomeDirectoryMapPath => _portalInfo.HomeDirectoryMapPath;

    public string HomeSystemDirectoryMapPath => _portalInfo.HomeSystemDirectoryMapPath;

    public int AdministratorId
    {
        get => _portalInfo.AdministratorId;
        set => _portalInfo.AdministratorId = value;
    }

    public int AdministratorRoleId
    {
        get => _portalInfo.AdministratorRoleId;
        set => _portalInfo.AdministratorRoleId = value;
    }

    public int AdminTabId
    {
        get => _portalInfo.AdminTabId;
        set => _portalInfo.AdminTabId = value;
    }

    public string? CrmVersion
    {
        get => _portalInfo.CrmVersion;
        set => _portalInfo.CrmVersion = value;
    }

    public string CultureCode
    {
        get => _portalInfo.CultureCode;
        set => _portalInfo.CultureCode = value;
    }

    public string DefaultLanguage
    {
        get => _portalInfo.DefaultLanguage;
        set => _portalInfo.DefaultLanguage = value;
    }

    public string? Description
    {
        get => _portalInfo.Description;
        set => _portalInfo.Description = value;
    }

    public string? Email
    {
        get => _portalInfo.Email;
        set => _portalInfo.Email = value;
    }

    public DateTime ExpiryDate
    {
        get => _portalInfo.ExpiryDate;
        set => _portalInfo.ExpiryDate = value;
    }

    public Guid GUID
    {
        get => _portalInfo.GUID;
        set => _portalInfo.GUID = value;
    }

    public int HomeTabId
    {
        get => _portalInfo.HomeTabId;
        set => _portalInfo.HomeTabId = value;
    }

    public int HostSpace
    {
        get => _portalInfo.HostSpace;
        set => _portalInfo.HostSpace = value;
    }

    public string? KeyWords
    {
        get => _portalInfo.KeyWords;
        set => _portalInfo.KeyWords = value;
    }

    public int LoginTabId
    {
        get => _portalInfo.LoginTabId;
        set => _portalInfo.LoginTabId = value;
    }

    public string? LogoFile
    {
        get => _portalInfo.LogoFile;
        set => _portalInfo.LogoFile = value;
    }

    public int PageQuota
    {
        get => _portalInfo.PageQuota;
        set => _portalInfo.PageQuota = value;
    }

    public int PortalId
    {
        get => _portalInfo.PortalId;
        set => _portalInfo.PortalId = value;
    }

    public int PortalGroupId
    {
        get => _portalInfo.PortalGroupId;
        set => _portalInfo.PortalGroupId = value;
    }

    public string PortalName
    {
        get => _portalInfo.PortalName;
        set => _portalInfo.PortalName = value;
    }

    public int RegisteredRoleId
    {
        get => _portalInfo.RegisteredRoleId;
        set => _portalInfo.RegisteredRoleId = value;
    }

    public int RegisterTabId
    {
        get => _portalInfo.RegisterTabId;
        set => _portalInfo.RegisterTabId = value;
    }

    public int SearchTabId
    {
        get => _portalInfo.SearchTabId;
        set => _portalInfo.SearchTabId = value;
    }

    public int Custom404TabId
    {
        get => _portalInfo.Custom404TabId;
        set => _portalInfo.Custom404TabId = value;
    }

    public int Custom500TabId
    {
        get => _portalInfo.Custom500TabId;
        set => _portalInfo.Custom500TabId = value;
    }

    public int TermsTabId
    {
        get => _portalInfo.TermsTabId;
        set => _portalInfo.TermsTabId = value;
    }

    public int PrivacyTabId
    {
        get => _portalInfo.PrivacyTabId;
        set => _portalInfo.PrivacyTabId = value;
    }

    public int SplashTabId
    {
        get => _portalInfo.SplashTabId;
        set => _portalInfo.SplashTabId = value;
    }

    public int SuperTabId
    {
        get => _portalInfo.SuperTabId;
        set => _portalInfo.SuperTabId = value;
    }

    public int UserQuota
    {
        get => _portalInfo.UserQuota;
        set => _portalInfo.UserQuota = value;
    }

    public int UserRegistration
    {
        get => _portalInfo.UserRegistration;
        set => _portalInfo.UserRegistration = value;
    }

    public int UserTabId
    {
        get => _portalInfo.UserTabId;
        set => _portalInfo.UserTabId = value;
    }

    public string? AdministratorRoleName
    {
        get => _portalInfo.AdministratorRoleName;
        set => _portalInfo.AdministratorRoleName = value;
    }

    public string? RegisteredRoleName
    {
        get => _portalInfo.RegisteredRoleName;
        set => _portalInfo.RegisteredRoleName = value;
    }

    public IReadOnlyDictionary<string, string> Settings { get; set; }
}
