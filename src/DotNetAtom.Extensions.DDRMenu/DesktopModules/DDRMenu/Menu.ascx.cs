using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using DotNetAtom.Entities;
using DotNetAtom.Framework;
using DotNetAtom.Tabs;
using DotNetAtom.TemplateEngine;
using DotNetAtom.TemplateEngine.Items;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using WebFormsCore;
using WebFormsCore.UI;

namespace DotNetAtom.DesktopModules.DDRMenu;

public partial class Menu(IMemoryCache memoryCache, ITabRouter tabRouter, IHostEnvironment environment) : PortalModuleBase
{
    private static readonly char[] AnyOf = { '/', '\\' };
    private DdrMenu? _menu;

    [ViewState] public string? MenuStyle { get; set; }

    [ViewState] public string? NodeSelector { get; set; }

    protected override async ValueTask OnPreRenderAsync(CancellationToken token)
    {
        await base.OnPreRenderAsync(token);

        if (MenuStyle is not null && PortalSettings.CurrentSkinPath is {} skinPath)
        {
            var fileProvider = environment.ContentRootFileProvider;
            var lastSeparator = MenuStyle.LastIndexOfAny(AnyOf);
            var name = lastSeparator == -1 ? MenuStyle : MenuStyle.Substring(lastSeparator + 1);
            var path = $"{skinPath}/{MenuStyle}/{name}.txt";
            var cacheKey = $"DdrMenu:{path}";

            if (memoryCache.TryGetValue(cacheKey, out _menu))
            {
                return;
            }

            var file = fileProvider.GetFileInfo(path);
            var changeToken = fileProvider.Watch(path);
            var content = file.Exists ? await file.ReadAllTextAsync() : null;
            var ddrMenu = content is null ? null : DdrMenu.Parse(content.AsSpan());

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                .AddExpirationToken(changeToken);

            _menu = memoryCache.Set(cacheKey, ddrMenu, cacheEntryOptions);
        }
    }

    public override ValueTask RenderAsync(HtmlTextWriter writer, CancellationToken token)
    {
        if (_menu == null)
        {
            return default;
        }

        var item = CreateRootItem();

        return _menu.RenderAsync(item, writer);
    }

    private RootItem CreateRootItem()
    {
        var portalId = PortalSettings.Portal.PortalId;
        var cultureCode = PortalSettings.Portal.CultureCode;
        var cacheKey = $"DdrMenu:RootItem:{portalId}:{cultureCode}";

        if (memoryCache.TryGetValue(cacheKey, out RootItem? rootItem) && rootItem is not null)
        {
            return rootItem;
        }

        var children = new List<IMenuItem>();

        foreach (var route in tabRouter.GetChildren(portalId, cultureCode, null))
        {
            if (!route.Tab.IsVisible)
            {
                continue;
            }

            if (!route.Tab.ParentId.HasValue)
            {
                children.Add(GetTabItem(route));
            }
        }

        var item = new RootItem(children);
        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(5))
            .AddExpirationToken(tabRouter.ChangeToken);

        return memoryCache.Set(cacheKey, item, cacheEntryOptions);
    }

    private TabItem GetTabItem(ITabRoute route)
    {
        var tabChildren = new List<IMenuItem>();
        var portalId = PortalSettings.Portal.PortalId;
        var cultureCode = PortalSettings.Portal.CultureCode;

        foreach (var child in tabRouter.GetChildren(portalId, cultureCode, route))
        {
            if (!child.Tab.IsVisible)
            {
                continue;
            }

            tabChildren.Add(GetTabItem(child));
        }

        string? path = null;

        var isActive = PortalSettings.ActiveTab != null && PortalSettings.ActiveTab.Equals(route.Tab);

        if (route.TryGetPath(out var pathString))
        {
            path = pathString;
        }

        return new TabItem(route.Tab, tabChildren, path, isActive);
    }
}