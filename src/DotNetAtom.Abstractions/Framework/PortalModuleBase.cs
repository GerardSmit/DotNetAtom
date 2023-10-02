using System;
using DotNetAtom.Modules;
using Microsoft.Extensions.DependencyInjection;
using WebFormsCore.UI;

namespace DotNetAtom.Framework;

public class PortalModuleBase : UserControlBase, IModuleControl
{
    private ModuleInstanceContext? _moduleContext;
    private string? _moduleName;

    public Control Control => this;

    public string ControlPath => throw new NotImplementedException();

    public string ControlName => _moduleName ??= Context.RequestServices
            .GetRequiredService<IModuleControlService>()
            .GetModuleName(this);

    public ModuleInstanceContext ModuleContext => _moduleContext ??= new ModuleInstanceContext(this);

    public string LocalResourceFile
    {
        get;
        set;
    } = "";
}
