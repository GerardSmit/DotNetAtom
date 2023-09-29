using System;
using DotNetAtom.Entities.Portals;

namespace DotNetAtom;

public interface IAtomContext : IDisposable
{
    Guid ApplicationId { get; set; }

    IPortalSettings PortalSettings { get; set; }
}
