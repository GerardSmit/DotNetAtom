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
    private readonly ITabRouter _tabRouter;
    private static readonly char[] AnyOf = { '/', '\\' };
    private DdrMenu? _menu;

    public Menu(ITabRouter tabRouter)
    {
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
        var portalId = PortalSettings.Portal.PortalId;
        var cultureCode = PortalSettings.Portal.CultureCode;

        foreach (var route in _tabRouter.GetChildren(portalId, cultureCode, null))
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

        return new RootItem(children);
    }

    private TabItem GetTabItem(ITabRoute route)
    {
        var tabChildren = new List<IMenuItem>();
        var portalId = PortalSettings.Portal.PortalId;
        var cultureCode = PortalSettings.Portal.CultureCode;

        foreach (var child in _tabRouter.GetChildren(portalId, cultureCode, route))
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