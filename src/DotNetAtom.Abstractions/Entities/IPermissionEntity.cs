namespace DotNetAtom.Entities;

public interface IPermissionEntity : IEntity
{
    int PermissionId { get; }

    int? RoleId { get; }

    int? UserId { get; }

    bool AllowAccess { get; }
}