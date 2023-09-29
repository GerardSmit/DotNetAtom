using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using DotNetAtom.Entities;
using DotNetAtom.Framework;
using DotNetAtom.Portals;
using DotNetAtom.Skins;
using Microsoft.Extensions.DependencyInjection;
using WebFormsCore.UI;

namespace DotNetAtom;

public partial class Default : Page
{
    private ITabInfo? _tab;
    private IPortalInfo? _portal;

    public string? CurrentSkinPath { get; set; }

    protected override async ValueTask OnInitAsync(CancellationToken token)
    {
        await base.OnInitAsync(token);

        Page.Csp.Enabled = true;
        Page.Csp.FontSrc.Add(new Uri("https://fonts.gstatic.com"));

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

        if (skinSrc is null && !portal.Settings.TryGetValue("DefaultPortalSkin", out skinSrc))
        {
            return;
        }

        var skinService = Context.RequestServices.GetRequiredService<ISkinService>();
        skinSrc = skinService.GetSkinSrc(skinSrc, portal);

        CurrentSkinPath = Path.GetDirectoryName(skinSrc);

        var skin = LoadControl(skinSrc);
        skin.ID = "dnn";

        _tab = tab;
        _portal = portal;

        if (Body != null)
        {
            await Body.Controls.AddAsync(skin);
        }
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
