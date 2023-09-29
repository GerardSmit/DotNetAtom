namespace DotNetAtom.Tabs;

public record struct RouteMatch(
    int? TabId,
    string CanonicalPath
);