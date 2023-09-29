using Dapper;

namespace DotNetAtom.Entities;

public class User : IEntity
{
    [DbValue(Name = "UserID")]
    public int Id { get; set; }

    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public string Username { get; set; } = default!;

    public string? Email { get; set; }

    public bool IsSuperUser { get; set; }
}
