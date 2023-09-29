using System.Collections.Generic;
using System.Threading.Tasks;

namespace DotNetAtom.Portals;

public interface IPortalRepository
{
    Task<IReadOnlyList<IPortalInfo>> GetPortalsAsync();
}
