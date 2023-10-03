﻿using System;
using System.Collections.Specialized;
using DotNetAtom.Entities;
using DotNetAtom.Security.Membership;

namespace DotNetAtom.Services.Authentication;

/// <summary>
/// The UserAuthenticatedEventArgs class provides a custom EventArgs object for the
/// UserAuthenticated event.
/// </summary>
public class UserAuthenticatedEventArgs : EventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserAuthenticatedEventArgs"/> class.
    /// All properties Constructor.
    /// </summary>
    /// <param name="user">The user being authenticated.</param>
    /// <param name="token">The user token.</param>
    /// <param name="status">The login status.</param>
    /// <param name="type">The type of Authentication.</param>
    public UserAuthenticatedEventArgs(IUserInfo user, string token, UserLoginStatus status, string type)
    {
        Profile = new NameValueCollection();
        Message = string.Empty;
        AutoRegister = false;
        Authenticated = true;
        User = user;
        LoginStatus = status;
        UserToken = token;
        AuthenticationType = type;
        RememberMe = false;
        Username = user.Username;
    }

    /// <summary>Gets or sets a value indicating whether gets and sets a flag that determines whether the User was authenticated.</summary>
    public bool Authenticated { get; set; }

    /// <summary>Gets or sets the Authentication Type.</summary>
    public string AuthenticationType { get; set; }

    /// <summary>Gets or sets a value indicating whether gets and sets a flag that determines whether the user should be automatically registered.</summary>
    public bool AutoRegister { get; set; }

    /// <summary>Gets or sets the Login Status.</summary>
    public UserLoginStatus LoginStatus { get; set; }

    /// <summary>Gets or sets the Message.</summary>
    public string Message { get; set; }

    /// <summary>Gets or sets the Profile.</summary>
    public NameValueCollection Profile { get; set; }

    /// <summary>Gets or sets a value indicating whether gets and sets the RememberMe setting.</summary>
    public bool RememberMe { get; set; }

    /// <summary>Gets or sets the User.</summary>
    public IUserInfo User { get; set; }

    /// <summary>Gets or sets the UserToken (the userid or authenticated id).</summary>
    public string UserToken { get; set; }

    /// <summary>Gets or sets the Username.</summary>
    public string Username { get; set; }
}