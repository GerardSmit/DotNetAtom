using System.Diagnostics.CodeAnalysis;
using DotNetAtom.Entities;
using HttpStack;

namespace DotNetAtom.Tabs;

public interface ITabRoute
{
    ITabInfo Tab { get; }

    ITabRoute? Parent { get; set; }

    bool IsMatch(IHttpRequest request);

    bool TryGetPath([NotNullWhen(true)] out string? path);
}