using DotNetAtom.Entities;

namespace DotNetAtom.Tabs;

public record struct RouteMatch(
    ITabInfo Tab,
    string CanonicalPath
);