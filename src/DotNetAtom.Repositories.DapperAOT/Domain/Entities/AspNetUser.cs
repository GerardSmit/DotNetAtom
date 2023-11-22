using System;
using Dapper;

namespace DotNetAtom.Entities;

public class AspNetUser : IAspNetUser
{
    public Guid ApplicationId { get; set; }

    public Guid UserId { get; set; }

    [DbValue(Name = "UserName")]
    public string Username { get; set; }

    [DbValue(Name = "LoweredUserName")]
    public string LoweredUsername { get; set; }

    public string? MobileAlias { get; set; }

    public bool IsAnonymous { get; set; }

    public DateTime LastActivityDate { get; set; }
}
