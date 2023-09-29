using Dapper;

namespace DotNetAtom.Entities;

public class UserRole : IEntity
{
    [DbValue(Name = "UserRoleID")]
    public int Id { get; set; }

    [DbValue(Name = "UserID")]
    public int UserId { get; set; }

    [DbValue(Name = "RoleID")]
    public int RoleId { get; set; }
}
