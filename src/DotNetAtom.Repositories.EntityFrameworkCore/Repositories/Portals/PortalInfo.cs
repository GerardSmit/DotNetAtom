using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DotNetAtom.Entities;

namespace DotNetAtom.Portals;

internal class PortalInfo : IPortalInfo
{
    public string? FooterText { get; set; }

    public required string HomeDirectory { get; set; }

    public required string HomeSystemDirectory { get; set; }

    public string? HomeDirectoryMapPath { get; }

    public string? HomeSystemDirectoryMapPath { get; }

    public int AdministratorId { get; set; }

    public int AdministratorRoleId { get; set; }

    public int AdminTabId { get; set; }

    public string? CrmVersion { get; set; }

    public required string CultureCode { get; set; }

    public required string DefaultLanguage { get; set; }

    public string? Description { get; set; }

    public string? Email { get; set; }

    public DateTime ExpiryDate { get; set; }

    public Guid GUID { get; set; }

    public int HomeTabId { get; set; }

    public int HostSpace { get; set; }

    public string? KeyWords { get; set; }

    public int LoginTabId { get; set; }

    public string? LogoFile { get; set; }

    public int PageQuota { get; set; }

    public int PortalId { get; set; }

    public int PortalGroupId { get; set; }

    public required string PortalName { get; set; }

    public int RegisteredRoleId { get; set; }

    public int RegisterTabId { get; set; }

    public int SearchTabId { get; set; }

    public int Custom404TabId { get; set; }

    public int Custom500TabId { get; set; }

    public int TermsTabId { get; set; }

    public int PrivacyTabId { get; set; }

    public int SplashTabId { get; set; }

    public int SuperTabId { get; set; }

    public int UserQuota { get; set; }

    public int UserRegistration { get; set; }

    public int UserTabId { get; set; }

    public string? AdministratorRoleName { get; set; }

    public string? RegisteredRoleName { get; set; }

    public required IReadOnlyDictionary<string, string> Settings { get; set; }

    public static IPortalInfo FromEntity(PortalLocalization portalLocalization, List<HostSetting> hostSettings)
    {
        var portal = portalLocalization.Portal;
        var administrator = portalLocalization.Portal.Administrator;
        var administratorRole = portalLocalization.Portal.AdministratorRole;
        var registeredRole = portalLocalization.Portal.RegisteredRole;
        var settings = portal.Settings
            .Where(setting => setting.CultureCode == portalLocalization.CultureCode)
            .ToDictionary(setting => setting.SettingName, setting => setting.SettingValue);

        foreach (var setting in portal.Settings.Where(setting => setting.CultureCode == null))
        {
            settings.TryAdd(setting.SettingName, setting.SettingValue);
        }

        foreach (var setting in hostSettings)
        {
            settings.TryAdd(setting.Name, setting.Value);
        }

        return new PortalInfo
        {
            Description = portalLocalization.Description,
            Email = administrator?.Email,
            ExpiryDate = portal.ExpiryDate ?? DateTime.MaxValue,
            FooterText = portalLocalization.FooterText,
            GUID = portal.Guid,
            HomeDirectory = portal.HomeDirectory,
            HomeSystemDirectory = Path.Combine(Directory.GetParent(portal.HomeDirectory)!.FullName, "_default"),
            KeyWords = portalLocalization.KeyWords,
            LogoFile = portalLocalization.LogoFile,
            PortalId = portal.Id,
            PortalName = portalLocalization.PortalName,
            AdministratorId = portal.AdministratorId ?? -1,
            AdministratorRoleId = portal.AdministratorRoleId ?? -1,
            AdminTabId = portalLocalization.AdminTabId ?? -1,
            CrmVersion = string.Empty,
            CultureCode = portalLocalization.CultureCode,
            DefaultLanguage = portal.DefaultLanguage,
            HostSpace = portal.HostSpace,
            LoginTabId = portalLocalization.LoginTabId ?? -1,
            PageQuota = portal.PageQuota,
            PortalGroupId = portal.PortalGroupId ?? -1,
            RegisteredRoleId = portal.RegisteredRoleId ?? -1,
            RegisterTabId = portalLocalization.RegisterTabId ?? -1,
            SearchTabId = portalLocalization.SearchTabId ?? -1,
            SplashTabId = portalLocalization.SplashTabId ?? -1,
            UserQuota = portal.UserQuota,
            UserRegistration = portal.UserRegistration,
            UserTabId = portalLocalization.UserTabId ?? -1,
            AdministratorRoleName = administratorRole?.Name,
            RegisteredRoleName = registeredRole?.Name,
            HomeTabId = portalLocalization.HomeTabId ?? -1,
            Settings = settings,

            // TODO: Implement these properties
            SuperTabId = -1,
            Custom404TabId = -1,
            Custom500TabId = -1,
            TermsTabId = -1,
            PrivacyTabId = -1
        };
    }

    public static void Update(PortalLocalization portalLocalization, IPortalInfo info)
    {
        var portal = portalLocalization.Portal;

        portalLocalization.Description = info.Description;
        portal.ExpiryDate = info.ExpiryDate == DateTime.MaxValue ? null : info.ExpiryDate;
        portalLocalization.FooterText = info.FooterText;
        portal.Guid = info.GUID;
        portal.HomeDirectory = info.HomeDirectory;
        portalLocalization.KeyWords = info.KeyWords;
        portalLocalization.LogoFile = info.LogoFile;
        portal.Id = info.PortalId;
        portalLocalization.PortalName = info.PortalName;
        portal.AdministratorId = info.AdministratorId == -1 ? null : info.AdministratorId;
        portal.AdministratorRoleId = info.AdministratorRoleId == -1 ? null : info.AdministratorRoleId;
        portalLocalization.AdminTabId = info.AdminTabId == -1 ? null : info.AdminTabId;
        portalLocalization.CultureCode = info.CultureCode;
        portal.DefaultLanguage = info.DefaultLanguage;
        portal.HostSpace = info.HostSpace;
        portalLocalization.LoginTabId = info.LoginTabId == -1 ? null : info.LoginTabId;
        portal.PageQuota = info.PageQuota;
        portal.PortalGroupId = info.PortalGroupId == -1 ? null : info.PortalGroupId;
        portal.RegisteredRoleId = info.RegisteredRoleId == -1 ? null : info.RegisteredRoleId;
        portalLocalization.RegisterTabId = info.RegisterTabId == -1 ? null : info.RegisterTabId;
        portalLocalization.SearchTabId = info.SearchTabId == -1 ? null : info.SearchTabId;
        portalLocalization.SplashTabId = info.SplashTabId == -1 ? null : info.SplashTabId;
        portal.UserQuota = info.UserQuota;
        portal.UserRegistration = info.UserRegistration;
        portalLocalization.UserTabId = info.UserTabId == -1 ? null : info.UserTabId;
        portalLocalization.HomeTabId = info.HomeTabId == -1 ? null : info.HomeTabId;
    }
}
