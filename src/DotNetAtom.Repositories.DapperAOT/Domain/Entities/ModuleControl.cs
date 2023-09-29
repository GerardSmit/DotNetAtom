using Dapper;

namespace DotNetAtom.Entities;

public class ModuleControl : IEntity, ITimestamp
{
    [DbValue(Name = "ModuleControlID")]
    public int Id { get; set; }

    [DbValue(Name = "ModuleDefID")]
    public int? ModuleDefinitionId { get; set; }

    public string? ControlKey { get; set; }

    public string? ControlTitle { get; set; }

    public string? ControlSrc { get; set; }

    public string? IconFile { get; set; }

    public int ControlType { get; set; }

    public int? ViewOrder { get; set; }

    public string? HelpUrl { get; set; }

    public bool SupportsPartialRendering { get; set; }

    [DbValue(Name = "CreatedByUserID")]
    public int? CreatedByUserId { get; set; }

    public DateTime? CreatedOnDate { get; set; }

    [DbValue(Name = "LastModifiedByUserID")]
    public int? LastModifiedByUserId { get; set; }

    public DateTime? LastModifiedOnDate { get; set; }

    public bool SupportsPopUps { get; set; }
}
