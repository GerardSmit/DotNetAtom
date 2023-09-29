using Dapper;

namespace DotNetAtom.Entities;

public class PackageType
{
    [DbValue(Name = "PackageType")]
    public required string Code { get; set; }

    public string Description { get; set; }

    public SecurityAccessLevel SecurityAccessLevel { get; set; }

    public string? EditorControlSrc { get; set; }
}

