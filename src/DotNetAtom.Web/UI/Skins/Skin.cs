using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DotNetAtom.Framework;
using DotNetAtom.Modules;
using DotNetAtom.Skins;
using DotNetAtom.UI.Containers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebFormsCore.UI;
using WebFormsCore.UI.HtmlControls;
using WebFormsCore.UI.WebControls;

namespace DotNetAtom.UI.Skins;

public partial class Skin : UserControlBase, INamingContainer
{
    public string? SkinPath => PortalSettings.CurrentSkinPath;

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
                paneControl.ID is null)
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
        var skinService = Context.RequestServices.GetRequiredService<ISkinService>();
        var moduleControlService = ServiceProvider.GetRequiredService<IModuleControlService>();
        var moduleService = Context.RequestServices.GetRequiredService<IModuleService>();
        var logger = ServiceProvider.GetRequiredService<ILogger<Skin>>();

        var ctl = Request.Query.TryGetValue("ctl", out var ctlValue)
            ? (StringKey)ctlValue[0]
            : default;

        var mid = Request.Query.TryGetValue("mid", out var midValue) && int.TryParse(midValue, out var midInt)
            ? midInt
            : -1;

        if (ctl.HasValue && mid == -1)
        {
            return;
        }

        foreach (var tabModule in Tab.TabModules)
        {
            if (tabModule.IsDeleted)
            {
                continue;
            }

            if (mid != -1 && tabModule.ModuleId != mid)
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

                var moduleHost = WebActivator.CreateControl<ModuleHost>();
                moduleHost.ModuleInfo = tabModule;
                await pane.Controls.AddAsync(moduleHost);

                var containerSrc = tabModule.ContainerSrc ?? Tab.ContainerSrc;

                if (containerSrc is null && Tab.TabSettings.TryGetValue("DefaultPortalContainer", out var defaultContainerSrc))
                {
                    containerSrc = defaultContainerSrc;
                }
                else if (containerSrc is null && PortalSettings.Portal.Settings.TryGetValue("DefaultPortalContainer", out var portalDefaultContainerSrc))
                {
                    containerSrc = portalDefaultContainerSrc;
                }

                var control = await moduleControlService.CreateModuleControlAsync(Page, PortalSettings, moduleHost, tabModule, controlKey);

                if (control != null)
                {
                    Control container;

                    moduleHost.ModuleControl = (control as IModuleControl)!;

                    if (containerSrc is not null)
                    {
                        var path = skinService.GetSkinSrc(containerSrc, PortalSettings.Portal);
                        var containerControl = LoadControl(path);

                        await moduleHost.Controls.AddAsync(containerControl);

                        if (containerControl is INamingContainer && containerControl.FindControl("ContentPane") is {} controlContainer)
                        {
                            if (controlContainer is HtmlGenericControl htmlControl)
                            {
                                var module = moduleService.GetDefinition(tabModule);

                                htmlControl.Attributes["class"] = $"DNNModuleContent Mod{module.DesktopModule.ModuleName}C";
                            }

                            container = controlContainer;
                        }
                        else
                        {
                            container = containerControl;
                        }
                    }
                    else
                    {
                        container = moduleHost;
                    }

                    await container.Controls.AddAsync(control);
                }
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
