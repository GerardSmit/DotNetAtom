using System;
using DotNetAtom.Entities;

namespace DotNetAtom.Tabs.Cache;

internal class TabSettingInfo : ITabSettingInfo
{
    public TabSettingInfo(TabSetting tabSetting)
    {
        TabId = tabSetting.TabId;
        SettingName = tabSetting.SettingName;
        SettingValue = tabSetting.SettingValue;
        CreatedOnDate = tabSetting.CreatedOnDate;
        LastModifiedOnDate = tabSetting.LastModifiedOnDate;
    }

    public int TabId { get; }
    public string SettingName { get; }
    public string SettingValue { get; }
    public DateTime? CreatedOnDate { get; }
    public DateTime? LastModifiedOnDate { get; }
}
