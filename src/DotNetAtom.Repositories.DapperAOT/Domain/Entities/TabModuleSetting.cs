using Dapper;

namespace DotNetAtom.Entities;

public class TabModuleSetting : ITimestamp
{
    [DbValue(Name = "TabModuleID")]
    public int TabModuleId { get; set; }

    public string SettingName { get; set; }

    public string SettingValue { get; set; }

    [DbValue(Name = "CreatedByUserID")]
    public int? CreatedByUserId { get; set; }

    public DateTime? CreatedOnDate { get; set; }

    [DbValue(Name = "LastModifiedByUserID")]
    public int? LastModifiedByUserId { get; set; }

    public DateTime? LastModifiedOnDate { get; set; }
}
