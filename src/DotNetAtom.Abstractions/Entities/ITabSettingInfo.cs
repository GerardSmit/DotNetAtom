using System;

namespace DotNetAtom.Entities;

public interface ITabSettingInfo
{
    int TabId { get; }

    string SettingName { get; }

    string SettingValue { get; }

    DateTime? CreatedOnDate { get; }

    DateTime? LastModifiedOnDate { get; }
}