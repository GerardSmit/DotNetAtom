using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using DotNetAtom.DesktopModules.DDRMenu.TemplateEngine;
using DotNetAtom.Entities;
using DotNetAtom.Framework;
using DotNetAtom.Tabs;
using Microsoft.Extensions.DependencyInjection;
using WebFormsCore;
using WebFormsCore.UI;

namespace DotNetAtom.DesktopModules.DDRMenu;

public partial class Menu : PortalModuleBase
{
    private readonly ITabService _tabService;
    private readonly ITabRouter _tabRouter;
    private static readonly char[] AnyOf = new[] { '/', '\\' };
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

        if (MenuStyle is not null && Page is Default { CurrentSkinPath: {} skinPath })
        {
            var lastSeparator = MenuStyle.LastIndexOfAny(AnyOf);
            var name = lastSeparator == -1 ? MenuStyle : MenuStyle.Substring(lastSeparator + 1);
            var path = Path.Combine(skinPath, MenuStyle, name + ".txt");
            var css = Path.Combine(skinPath, MenuStyle, name + ".css");

            if (File.Exists(path))
            {
                var menu = DdrMenu.Parse(File.ReadAllText(path));
                _menu = menu;
            }

            if (File.Exists(css))
            {
                Page.ClientScript.RegisterStartupStyle(typeof(Menu), "Test", File.ReadAllText(css), true);
            }
        }
    }

    public override ValueTask RenderAsync(HtmlTextWriter writer, CancellationToken token)
    {
        if (_menu == null)
        {
            return default;
        }

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

        return _menu.RenderAsync(new RootItem(children), writer);
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

    public class TabItem : RootItem
    {
        private readonly ITabInfo _tabInfo;
        private readonly string? _url;
        private readonly bool _isActive;

        public TabItem(ITabInfo tabInfo, IReadOnlyList<IMenuItem> children, string? url, bool isActive)
            : base(children)
        {
            _tabInfo = tabInfo;
            _url = url;
            _isActive = isActive;
        }

        public override object? GetNode(string key)
        {
            if (key == "TEXT")
            {
                return _tabInfo.TabName;
            }

            if (key == "URL")
            {
                return _url;
            }

            return base.GetNode(key);
        }

        public override bool TestNode(string key)
        {
            if (key == "ENABLED")
            {
                return !_tabInfo.DisableLink;
            }

            if (key == "SELECTED")
            {
                return _isActive;
            }

            return base.TestNode(key);
        }
    }

    public class RootItem : IMenuItem
    {
        private readonly IReadOnlyList<IMenuItem> _children;

        public RootItem(IReadOnlyList<IMenuItem> children)
        {
            _children = children;
        }

        public virtual object? GetNode(string key)
        {
            if (key == "NODE")
            {
                return _children;
            }

            return null;
        }

        public virtual bool TestNode(string key)
        {
            if (key == "NODE")
            {
                return _children.Count > 0;
            }

            return GetNode(key) is not null;
        }
    }
}
