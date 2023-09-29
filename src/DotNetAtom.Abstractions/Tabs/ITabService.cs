using System.Collections.Generic;
using System.Threading.Tasks;
using DotNetAtom.Entities;

namespace DotNetAtom.Tabs;

public interface ITabService
{
    IReadOnlyCollection<ITabInfo> Tabs { get; }

    ITabInfo GetTab(int tabId);

    Task LoadAsync();
}
