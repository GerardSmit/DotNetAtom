using System.Threading.Tasks;
using DotNetAtom.Entities;
using DotNetAtom.Portals;

namespace DotNetAtom;

public interface IAtomContextFactory
{
    ValueTask<IAtomContext> CreateAsync(IPortalInfo portal, ITabInfo? tab = null);
}
