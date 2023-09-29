using System;
using System.ComponentModel;
using DotNetAtom.Entities.Portals;
using DotNetAtom.Framework;
using WebFormsCore.UI;

namespace DotNetAtom.UI.Skins;

public class SkinObjectBase : Control
{
    /// <summary>Gets the portal Settings for this Skin Control.</summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]

    public IPortalSettings PortalSettings => Context.Features.Get<IAtomFeature>()?.AtomContext.PortalSettings ?? throw new InvalidOperationException();
}
