using System.Collections.Generic;
using HttpStack;

namespace DotNetAtom.Tabs;

public interface IRouteCollection : IReadOnlyList<ITabRoute>
{
    bool TryMatch(PathString path, out RouteMatch match);

    bool TryGetPath(int tabId, out PathString path);
}