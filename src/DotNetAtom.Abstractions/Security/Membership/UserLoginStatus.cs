using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using DotNetAtom.Entities;

namespace DotNetAtom.Security.Membership;

public enum UserLoginStatus
{
    LOGIN_FAILURE = 0,

    LOGIN_SUCCESS = 1,

    LOGIN_SUPERUSER = 2,

    LOGIN_USERLOCKEDOUT = 3,

    LOGIN_USERNOTAPPROVED = 4,

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Deprecated in DotNetNuke 9.8.1.")]
    LOGIN_INSECUREADMINPASSWORD = 5,

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Deprecated in DotNetNuke 9.8.1..")]
    LOGIN_INSECUREHOSTPASSWORD = 6,
}

public record struct LoginResult(UserLoginStatus Status, IUserInfo? UserInfo)
{
    [MemberNotNullWhen(true, nameof(UserInfo))]
    public bool IsSuccess => Status is UserLoginStatus.LOGIN_SUCCESS or UserLoginStatus.LOGIN_SUPERUSER;
}