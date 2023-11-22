using System;
using System.ComponentModel;
using DotNetAtom.Entities.Portals;
using DotNetAtom.Framework;
using DotNetAtom.Modules;
using WebFormsCore.UI;

namespace DotNetAtom.UI.Skins;

public class SkinObjectBase : Control
{
    private IModuleControl? _moduleControl;

    /// <summary>Gets the portal Settings for this Skin Control.</summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]

    public IPortalSettings PortalSettings => Context.Features.Get<IAtomFeature>()?.AtomContext.PortalSettings ?? throw new InvalidOperationException();

    public IModuleControl ModuleControl => _moduleControl ??= this.FindParent<ModuleHost>()?.ModuleControl!;
}
