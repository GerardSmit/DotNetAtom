using Dapper;

namespace DotNetAtom.Entities;

public class ModuleDefinition : IEntity, ITimestamp
{
    /// <inheritdoc />
    [DbValue(Name = "ModuleDefID")]
    public int Id { get; set; }

    public string FriendlyName { get; set; }

    [DbValue(Name = "DesktopModuleID")]
    public int DesktopModuleId { get; set; }

    public int DefaultCacheTime { get; set; }

    [DbValue(Name = "CreatedByUserID")]
    public int? CreatedByUserId { get; set; }

    public DateTime? CreatedOnDate { get; set; }

    [DbValue(Name = "LastModifiedByUserID")]
    public int? LastModifiedByUserId { get; set; }

    public DateTime? LastModifiedOnDate { get; set; }
}
