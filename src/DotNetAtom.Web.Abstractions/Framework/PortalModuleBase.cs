using System.IO;
using DotNetAtom.Entities;
using DotNetAtom.Localization;
using DotNetAtom.Modules;
using Microsoft.Extensions.DependencyInjection;
using WebFormsCore.UI;

namespace DotNetAtom.Framework;

public class PortalModuleBase : UserControlBase, IModuleControl
{
    private ModuleInstanceContext? _moduleContext;
    private string? _moduleName;
    private string? _localResourceFile;
    private ILocalizationService? _localizationService;

    public Control Control => this;

    public string? ControlPath => AppFullPath;

    public string ControlName => _moduleName ??= Context.RequestServices
            .GetRequiredService<IModuleControlService>()
            .GetModuleName(this);

    public ModuleInstanceContext ModuleContext => _moduleContext ??= new ModuleInstanceContext(this);

    public string? LocalResourceFile
    {
	    get => _localResourceFile ??= ResolveLocalResourceFile();
	    set => _localResourceFile = value;
    }

    protected string GetString(string resourceName)
    {
	    _localizationService ??= Context.RequestServices.GetRequiredService<ILocalizationService>();

	    return _localizationService.GetString(resourceName, LocalResourceFile);
    }

    private string? ResolveLocalResourceFile()
    {
	    if (AppFullPath is null)
	    {
		    return null;
	    }

	    var fileName = Path.GetFileName(AppFullPath);
	    var directory = Path.GetDirectoryName(AppFullPath);

	    if (directory is null)
	    {
		    return null;
	    }

	    var resxFileName = $"{fileName}.resx";

	    return Path.Combine(directory, "App_LocalResources", resxFileName);
    }
}
