using System;

namespace DotNetAtom.Entities;

public class AspNetMembership
{
    public Guid ApplicationId { get; set; }

    public Guid UserId { get; set; }

    public string Password { get; set; } = default!;

    public string PasswordSalt { get; set; } = default!;

    public string? Email { get; set; }

    public string? LoweredEmail { get; set; }
}
