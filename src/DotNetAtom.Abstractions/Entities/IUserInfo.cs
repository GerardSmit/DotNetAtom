using System;
using System.ComponentModel;

namespace DotNetAtom.Entities;

public interface IUserInfo
{
    /// <summary>Gets or sets the User Id.</summary>
    [Browsable(false)]
    public int UserId { get; set; }

    /// <summary>Gets or sets the Display Name.</summary>
    public string DisplayName { get; set; }

    /// <summary>Gets or sets the Email Address.</summary>
    public string Email { get; set; }

    /// <summary>Gets or sets the User Name.</summary>
    public string Username { get; set; }

    /// <summary>Gets or sets the PortalId.</summary>
    public int PortalId { get; set; }

    /// <summary>Gets or sets a value indicating whether the user has agreed to the terms and conditions.</summary>
    public bool HasAgreedToTerms { get; set; }

    /// <summary>Gets or sets when the user last agreed to the terms and conditions.</summary>
    public DateTime HasAgreedToTermsOn { get; set; }

    /// <summary>Gets or sets a value indicating whether the user has requested they be removed from the site.</summary>
    public bool RequestsRemoval { get; set; }
}
