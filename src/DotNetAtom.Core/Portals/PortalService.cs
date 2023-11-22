using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetAtom.Portals;

internal class PortalService : IPortalService
{
    private readonly IPortalRepository _repository;
    private Dictionary<PortalCultureKey, IPortalInfo> _portals = new(PortalCultureKey.OrdinalIgnoreCase);
    private Dictionary<int, string> _defaultCultures = new();

    public PortalService(IPortalRepository repository)
    {
        _repository = repository;
    }

    public IReadOnlyCollection<IPortalInfo> Portals { get; private set; } = Array.Empty<IPortalInfo>();
    public IReadOnlyCollection<IPortalInfo> PortalCultures => _portals.Values;

    public IPortalInfo GetPortal(int portalId, string? culture = null)
    {
        if (culture == null && _defaultCultures.TryGetValue(portalId, out var defaultCulture))
        {
            culture = defaultCulture;
        }

        if (!_portals.TryGetValue(new PortalCultureKey(portalId, culture), out var portal))
        {
            throw new ArgumentException("The portal does not exist.", nameof(portalId));
        }

        return portal;
    }

    public IEnumerable<IPortalInfo> GetPortalCultures(int portalId)
    {
        return _portals
            .Where(p => p.Key.PortalId == portalId)
            .Select(p => p.Value);
    }

    public string GetDefaultCulture(int portalId)
    {
        return _defaultCultures.TryGetValue(portalId, out var defaultCulture)
            ? defaultCulture
            : "en-US";
    }

    public async Task LoadAsync()
    {
        var portals = await _repository.GetPortalsAsync();

        var result = new Dictionary<PortalCultureKey, IPortalInfo>(PortalCultureKey.OrdinalIgnoreCase);
        var defaultCultures = new Dictionary<int, string>();
        var portalIds = new HashSet<int>();

        foreach (var portal in portals)
        {
            portalIds.Add(portal.PortalId);

            if (!defaultCultures.ContainsKey(portal.PortalId))
            {
                defaultCultures.Add(portal.PortalId, portal.DefaultLanguage);
            }

            var key = new PortalCultureKey(portal.PortalId, portal.CultureCode);

            result[key] = portal;
        }

        _portals = result;
        _defaultCultures = defaultCultures;

        Portals = portalIds
            .Select(portalId => GetPortal(portalId))
            .ToList();
    }
}
