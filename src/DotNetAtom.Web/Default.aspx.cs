using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using DotNetAtom.Entities;
using DotNetAtom.Framework;
using DotNetAtom.Portals;
using DotNetAtom.Providers;
using DotNetAtom.Skins;
using DotNetAtom.Web.Client.ClientResourceManagement;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebFormsCore;
using WebFormsCore.Features;
using WebFormsCore.UI;

namespace DotNetAtom;

public partial class Default : Page
{
    private ITabInfo? _tab;
    private IPortalInfo? _portal;

    protected override async ValueTask OnInitAsync(CancellationToken token)
    {
        await base.OnInitAsync(token);

        await LoadSkinAsync();
    }

    private async Task LoadSkinAsync()
    {
        var settings = Context.Features.Get<IAtomFeature>()?.AtomContext.PortalSettings;

        if (settings is not { ActiveTab: { IsDeleted: false } tab, Portal: var portal })
        {
            return;
        }

        var skinSrc = tab.SkinSrc;
        var forceDefaultSkin = Request.Query.ContainsKey("ctl");

        if ((forceDefaultSkin || skinSrc is null) && !portal.Settings.TryGetValue("DefaultPortalSkin", out skinSrc))
        {
            return;
        }

        var skinService = Context.RequestServices.GetRequiredService<ISkinService>();
        skinSrc = skinService.GetSkinSrc(skinSrc, portal);

        var webFormsEnvironment = Context.RequestServices.GetRequiredService<IWebFormsEnvironment>();

        if (webFormsEnvironment.ContentRootPath is { } contentRootPath)
        {
            var directory = Path.GetDirectoryName(skinSrc);

            settings.CurrentSkinDirectory = directory;

            if (directory is not null && directory.StartsWith(contentRootPath, StringComparison.OrdinalIgnoreCase))
            {
                settings.CurrentSkinPath = directory.Substring(contentRootPath.Length).Replace('\\', '/');
            }

            if (settings.CurrentSkinPath is not null)
            {
                var skinCss = Path.Combine(settings.CurrentSkinPath, "skin.css");

                if (skinCss.StartsWith(contentRootPath, StringComparison.OrdinalIgnoreCase) &&
                    File.Exists(skinCss))
                {
                    Page.ClientScript.RegisterStartupStyleLink(typeof(Default), "SkinCss", skinCss.Substring(contentRootPath.Length).Replace('\\', '/'));
                }
            }

            if (File.Exists(Path.Combine(contentRootPath, "Portals", "_default", "default.css")))
            {
                var collection = Context.RequestServices.GetRequiredService<IClientDependencyCollection>();

                collection.Add(new DnnCssInclude
                {
                    FilePath = "/Portals/_default/default.css"
                });
            }
        }

        var skin = LoadControl(skinSrc);
        skin.ID = "dnn";

        _tab = tab;
        _portal = portal;

        if (Body != null)
        {
            await Body.Controls.AddAsync(skin);
        }

        var titleProvider = Context.RequestServices.GetRequiredService<ITabTitleProvider>();

        title.Text = await titleProvider.GetTitleAsync(tab);
    }

    protected override ValueTask OnPreRenderAsync(CancellationToken token)
    {
        if (_tab != null && _portal != null && _tab.TabSettings.TryGetValue("CustomStylesheet", out var customStylesheet))
        {
            Page.ClientScript.RegisterStartupStyleLink(typeof(Default), "CustomStylesheet", $"/Portals/{_portal.PortalId}/{customStylesheet}");
        }

        return base.OnPreRenderAsync(token);
    }
}
