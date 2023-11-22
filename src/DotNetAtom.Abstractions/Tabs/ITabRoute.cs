using System.Diagnostics.CodeAnalysis;
using DotNetAtom.Entities;

namespace DotNetAtom.Tabs;

public interface ITabRoute
{
    ITabInfo Tab { get; }

    ITabRoute? Parent { get; set; }

    bool IsMatch(string path);

    bool TryGetPath([NotNullWhen(true)] out string? path);
}