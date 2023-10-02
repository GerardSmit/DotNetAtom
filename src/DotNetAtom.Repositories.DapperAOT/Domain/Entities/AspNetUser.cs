using System;
using Dapper;

namespace DotNetAtom.Entities;

public class AspNetUser
{
    public Guid ApplicationId { get; set; }

    public Guid UserId { get; set; }

    [DbValue(Name = "UserName")]
    public string Username { get; set; }

    [DbValue(Name = "LoweredUserName")]
    public string LoweredUsername { get; set; }
}
