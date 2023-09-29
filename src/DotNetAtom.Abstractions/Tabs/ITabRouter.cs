using System.Threading.Tasks;
using HttpStack;

namespace DotNetAtom.Tabs;

public interface ITabRouter
{
    IRouteCollection GetTabCollection(int portalId, string? cultureCode = null);

    bool Match(int? portalId, string? cultureCode, PathString path, out RouteMatch match);

    bool TryGetPath(int? portalId, string? cultureCode, int tabId, out PathString match);

    Task LoadAsync();
}