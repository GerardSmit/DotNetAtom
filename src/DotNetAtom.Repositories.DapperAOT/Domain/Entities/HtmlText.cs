using Dapper;

namespace DotNetAtom.Entities;

public class HtmlText : IEntity, ITimestamp
{
    [DbValue(Name = "ItemID")]
    public int Id { get; set; }

    [DbValue(Name = "ModuleId")]
    public int ModuleId { get; set; }

    public string? Content { get; set; }

    public int? Version { get; set; }

    public int? StateId { get; set; }

    public bool? IsPublished { get; set; }

    [DbValue(Name = "CreatedByUserID")]
    public int? CreatedByUserId { get; set; }

    public DateTime? CreatedOnDate { get; set; }

    [DbValue(Name = "LastModifiedByUserID")]
    public int? LastModifiedByUserId { get; set; }

    public DateTime? LastModifiedOnDate { get; set; }

    public string? Summary { get; set; }
}