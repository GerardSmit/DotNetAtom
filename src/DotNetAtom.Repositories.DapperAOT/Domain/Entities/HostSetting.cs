﻿using Dapper;

namespace DotNetAtom.Entities;

public class HostSetting : ITimestamp
{
    [DbValue(Name = "SettingName")]
    public string Name { get; set; }

    [DbValue(Name = "SettingNameValue")]
    public string Value { get; set; }

    public bool SettingIsSecure { get; set; }

    [DbValue(Name = "CreatedByUserID")]
    public int? CreatedByUserId { get; set; }

    public DateTime? CreatedOnDate { get; set; }

    [DbValue(Name = "LastModifiedByUserID")]
    public int? LastModifiedByUserId { get; set; }

    public DateTime? LastModifiedOnDate { get; set; }
}