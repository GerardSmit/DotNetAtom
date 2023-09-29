using System;
using Ben.Collections.Specialized;
using DotNetAtom.Modules;
using WebFormsCore.UI;

namespace DotNetAtom.Framework;

public class PortalModuleBase : UserControlBase, IModuleControl
{
    private ModuleInstanceContext? _moduleContext;
    private string? _moduleName;

    public Control Control => this;

    public string ControlPath => throw new NotImplementedException();

    public string ControlName
    {
        get
        {
            if (_moduleName is not null)
            {
                return _moduleName;
            }

            var name = GetType().Name;

            if (name.Contains("."))
            {
                Span<char> span = stackalloc char[name.Length];

                for (var i = 0; i < name.Length; i++)
                {
                    var c = name[i];
                    span[i] = c == '.' ? '_' : c;
                }

                _moduleName = InternPool.Shared.Intern(span);
            }
            else
            {
                _moduleName = name;
            }

            return _moduleName;
        }
    }

    public ModuleInstanceContext ModuleContext => _moduleContext ??= new ModuleInstanceContext(this);

    public string LocalResourceFile
    {
        get;
        set;
    }
}
