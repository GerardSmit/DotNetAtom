using HttpStack;

namespace DotNetAtom.Tabs;

public interface ITabRoute
{
    bool IsMatch(PathString path, out RouteMatch match);

    bool TryGetPath(int tabId, out PathString path);
}