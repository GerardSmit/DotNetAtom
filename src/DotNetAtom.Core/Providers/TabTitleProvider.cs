using System.Threading.Tasks;
using DotNetAtom.Entities;

namespace DotNetAtom.Providers;

public class TabTitleProvider : ITabTitleProvider
{
    public ValueTask<string> GetTitleAsync(ITabInfo tabInfo)
    {
        return new ValueTask<string>(tabInfo.TabName);
    }
}
