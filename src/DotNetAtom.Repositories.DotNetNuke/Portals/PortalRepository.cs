using DotNetNuke.Entities.Portals;

namespace DotNetAtom.Portals;

public class PortalRepository : IPortalRepository
{
    private readonly IPortalController _portalController;

    public PortalRepository(IPortalController portalController)
    {
        _portalController = portalController;
    }

    public Task<IReadOnlyList<IPortalInfo>> GetPortalsAsync()
    {
        return Task.FromResult<IReadOnlyList<IPortalInfo>>(
            _portalController.GetPortals()
                .Cast<DotNetNuke.Abstractions.Portals.IPortalInfo>()
                .Select(portal => new PortalInfoWrapper(portal, _portalController.GetPortalSettings(portal.PortalId, portal.CultureCode)))
                .Cast<IPortalInfo>()
                .ToArray()
        );
    }
}
