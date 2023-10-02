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
using WebFormsCore;
using WebFormsCore.UI;

namespace DotNetAtom.DesktopModules.DDRMenu;

public partial class Menu : PortalModuleBase
{
    private readonly ITabService _tabService;
    private readonly ITabRouter _tabRouter;
    private static readonly char[] AnyOf = { '/', '\\' };
    private DdrMenu? _menu;

    public Menu(ITabService tabService, ITabRouter tabRouter)
    {
        _tabService = tabService;
        _tabRouter = tabRouter;
    }

    [ViewState] public string? MenuStyle { get; set; }

    [ViewState] public string? NodeSelector { get; set; }

    protected override void OnPreRender(EventArgs args)
    {
        base.OnPreRender(args);

        if (MenuStyle is not null && PortalSettings.CurrentSkinPath is {} skinPath)
        {
            var lastSeparator = MenuStyle.LastIndexOfAny(AnyOf);
            var name = lastSeparator == -1 ? MenuStyle : MenuStyle.Substring(lastSeparator + 1);
            var path = Path.Combine(skinPath, MenuStyle, name + ".txt");
            var css = Path.Combine(skinPath, MenuStyle, name + ".css");

            if (File.Exists(path))
            {
                var menu = DdrMenu.Parse(File.ReadAllText(path).AsSpan());

                _menu = menu;
            }

            if (File.Exists(css))
            {
                Page.ClientScript.RegisterStartupStyle(typeof(Menu), MenuStyle, File.ReadAllText(css), true);
            }
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
        var children = new List<IMenuItem>();

        foreach (var tab in _tabService.Tabs)
        {
            if (!tab.IsVisible)
            {
                continue;
            }

            if (!tab.ParentId.HasValue)
            {
                children.Add(GetTabItem(tab));
            }
        }

        return new RootItem(children);
    }

    private TabItem GetTabItem(ITabInfo tabInfo)
    {
        var tabChildren = new List<IMenuItem>();

        foreach (var child in _tabService.Tabs)
        {
            if (!child.IsVisible)
            {
                continue;
            }

            if (child.ParentId == tabInfo.TabId)
            {
                tabChildren.Add(GetTabItem(child));
            }
        }

        string? path = null;

        var portalId = PortalSettings.Portal.PortalId;
        var cultureCode = PortalSettings.Portal.CultureCode;
        var isActive = PortalSettings.ActiveTab?.TabId == tabInfo.TabId;

        if (_tabRouter.TryGetPath(portalId, cultureCode, tabInfo.TabId, out var pathString))
        {
            path = pathString.Value;
        }

        return new TabItem(tabInfo, tabChildren, path, isActive);
    }
}