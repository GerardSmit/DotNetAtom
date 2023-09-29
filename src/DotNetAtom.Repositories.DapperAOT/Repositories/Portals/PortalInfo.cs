using System;
using System.Collections.Generic;
using System.Linq;
using DotNetAtom.Entities;

namespace DotNetAtom.Portals;

internal class PortalInfo : IPortalInfo
{
    public string? FooterText { get; set; }

    public string? HomeDirectory { get; set; }

    public string? HomeSystemDirectory { get; }

    public string? HomeDirectoryMapPath { get; }

    public string? HomeSystemDirectoryMapPath { get; }

    public int AdministratorId { get; set; }

    public int AdministratorRoleId { get; set; }

    public int AdminTabId { get; set; }

    public string? CrmVersion { get; set; }

    public string? CultureCode { get; set; }

    public string? DefaultLanguage { get; set; }

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

    public string? PortalName { get; set; }

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

    public static IPortalInfo FromEntity(
        PortalLocalization portalLocalization,
        Portal portal,
        User? administrator,
        Role? administratorRole,
        Role? registeredRole,
        IReadOnlyList<PortalSetting> portalSettings)

    {
        var settings = portalSettings
            .Where(setting => setting.CultureCode == portalLocalization.CultureCode)
            .ToDictionary(setting => setting.SettingName, setting => setting.SettingValue);

        foreach (var setting in portalSettings.Where(setting => setting.CultureCode == null))
        {
#if NET6_0_OR_GREATER
            settings.TryAdd(setting.SettingName, setting.SettingValue);
#else
            if (!settings.ContainsKey(setting.SettingName))
            {
                settings.Add(setting.SettingName, setting.SettingValue);
            }
#endif
        }

        return new PortalInfo
        {
            Description = portalLocalization.Description,
            Email = administrator?.Email,
            ExpiryDate = portal.ExpiryDate ?? DateTime.MaxValue,
            FooterText = portalLocalization.FooterText,
            GUID = portal.Guid,
            HomeDirectory = portal.HomeDirectory,
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
}
