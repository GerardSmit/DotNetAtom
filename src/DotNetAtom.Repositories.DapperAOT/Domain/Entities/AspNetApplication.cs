using Dapper;

namespace DotNetAtom.Entities;

public partial class AspNetApplication
{
    [DbValue(Name = "ApplicationId")]
    public Guid Id { get; set; }

    public string ApplicationName { get; set; }

    public string LoweredApplicationName { get; set; }
}

