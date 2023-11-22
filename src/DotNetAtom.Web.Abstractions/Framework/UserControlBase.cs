using System;
using DotNetAtom.Application;
using DotNetAtom.Entities;
using DotNetAtom.Entities.Portals;
using Microsoft.Extensions.DependencyInjection;
using WebFormsCore.UI;

namespace DotNetAtom.Framework;

public class UserControlBase : Control
{
    private IAtomContext? _atomContext;

    public IAtomContext AtomContext
        => _atomContext ??= Context.Features.Get<IAtomFeature>()?.AtomContext ?? throw new InvalidOperationException("Atom context not found");

    public IPortalSettings PortalSettings => AtomContext.PortalSettings;

    public int PortalId => PortalSettings.Portal.PortalId;

    public ITabInfo Tab => PortalSettings.ActiveTab!;

    public IUserInfo User => PortalSettings.User;

    public int UserId => User.UserId;
}
