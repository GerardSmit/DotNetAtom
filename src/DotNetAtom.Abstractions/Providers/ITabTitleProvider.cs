using System.Threading.Tasks;
using DotNetAtom.Entities;

namespace DotNetAtom.Providers;

public interface ITabTitleProvider
{
    /// <summary>
    /// Gets the title for the tab.
    /// </summary>
    /// <param name="tabInfo">The tab.</param>
    /// <returns>The title.</returns>
    ValueTask<string> GetTitleAsync(ITabInfo tabInfo);
}
