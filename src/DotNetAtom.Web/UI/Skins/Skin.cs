using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DotNetAtom.Framework;
using DotNetAtom.Modules;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebFormsCore.UI;
using WebFormsCore.UI.HtmlControls;
using WebFormsCore.UI.WebControls;

namespace DotNetAtom.UI.Skins;

public partial class Skin : UserControlBase, INamingContainer
{
    private static readonly List<string> PaneTagNames = new()
    {
        "td",
        "div",
        "span",
        "p",
        "section",
        "header",
        "footer",
        "main",
        "article",
        "aside"
    };

    private readonly Dictionary<string, HtmlContainerControl> _panes = new();
    private HtmlContainerControl? _defaultPane;

    protected override async ValueTask OnInitAsync(CancellationToken token)
    {
        await base.OnInitAsync(token);

        LoadPanes();
        await LoadModulesAsync();
    }

    private void LoadPanes()
    {
        foreach (var control in this.EnumerateControls())
        {
            if (control is not HtmlContainerControl paneControl)
            {
                continue;
            }

            if (paneControl is LiteralHtmlControl ||
                control.NamingContainer != this ||
                !PaneTagNames.Contains(paneControl.TagName, StringComparer.OrdinalIgnoreCase) ||
                string.IsNullOrEmpty(paneControl.ID))
            {
                continue;
            }

            _panes.Add(paneControl.ID, paneControl);
        }

        _defaultPane ??= _panes.TryGetValue("ContentPane", out var contentPane)
            ? contentPane
            : _panes.Values.FirstOrDefault();
    }

    private async Task LoadModulesAsync()
    {
        var moduleService = ServiceProvider.GetRequiredService<IModuleControlService>();
        var logger = ServiceProvider.GetRequiredService<ILogger<Skin>>();

        var mid = Request.Query.TryGetValue("mid", out var midValue) && int.TryParse(midValue, out var midInt)
            ? midInt
            : -1;

        var ctl = Request.Query.TryGetValue("ctl", out var ctlValue)
            ? (StringKey)ctlValue[0]
            : default;

        foreach (var tabModule in Tab.TabModules)
        {
            if (tabModule.IsDeleted)
            {
                continue;
            }

            var controlKey = tabModule.ModuleId == mid ? ctl : default;

            try
            {
                if (!_panes.TryGetValue(tabModule.PaneName, out var pane))
                {
                    pane = _defaultPane;
                }

                if (pane is null)
                {
                    throw new InvalidOperationException("No pane found for module");
                }

                var moduleContainer = WebActivator.CreateControl<ModuleContainer>();
                moduleContainer.ModuleInfo = tabModule;

                var control = await moduleService.CreateModuleControlAsync(Page, PortalSettings, tabModule, controlKey);

                if (control != null)
                {
                    await moduleContainer.Controls.AddAsync(control);
                }

                await pane.Controls.AddAsync(moduleContainer);
            }
            catch (Exception e)
            {
                logger.LogError(
                    e,
                    "Failed to load control with key '{ControlKey}' for module {ModuleId}",
                    ctl,
                    tabModule.ModuleId);
            }
        }
    }
}
