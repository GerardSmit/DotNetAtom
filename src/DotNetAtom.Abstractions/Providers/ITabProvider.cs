using System.Collections.Generic;
using System.Threading.Tasks;
using DotNetAtom.Entities;

namespace DotNetAtom.Providers;

public interface ITabProvider
{
    ValueTask<IReadOnlyList<ITabInfo>> GetTabs();
}
