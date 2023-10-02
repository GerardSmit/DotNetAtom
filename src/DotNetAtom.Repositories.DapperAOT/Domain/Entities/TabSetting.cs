using System;
using Dapper;

namespace DotNetAtom.Entities;

public class TabSetting : ITimestamp
{
    [DbValue(Name = "TabId")]
    public int TabId { get; set; }

    public string SettingName { get; set; } = default!;

    public string SettingValue { get; set; } = default!;

    [DbValue(Name = "CreatedByUserID")]
    public int? CreatedByUserId { get; set; }

    public DateTime? CreatedOnDate { get; set; }

    [DbValue(Name = "LastModifiedByUserID")]
    public int? LastModifiedByUserId { get; set; }

    public DateTime? LastModifiedOnDate { get; set; }
}
