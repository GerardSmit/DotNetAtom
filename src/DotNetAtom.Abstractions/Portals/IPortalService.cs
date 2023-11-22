using System.Collections.Generic;
using System.Threading.Tasks;

namespace DotNetAtom.Portals;

public interface IPortalService
{
    IReadOnlyCollection<IPortalInfo> Portals { get; }

    IReadOnlyCollection<IPortalInfo> PortalCultures { get; }

    IPortalInfo GetPortal(int portalId, string? culture = null);

    IEnumerable<IPortalInfo> GetPortalCultures(int portalId);

    string GetDefaultCulture(int portalId);

    Task LoadAsync();
}
