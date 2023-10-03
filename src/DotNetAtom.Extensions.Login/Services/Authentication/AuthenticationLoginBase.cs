using System.Threading.Tasks;
using DotNetAtom.Framework;
using WebFormsCore;

namespace DotNetAtom.Services.Authentication;

 /// <summary>
    /// The AuthenticationLoginBase class provides a bas class for Authentication
    /// Login controls.
    /// </summary>
    public abstract partial class AuthenticationLoginBase : UserModuleBase
    {
        /// <summary>Initializes a new instance of the <see cref="AuthenticationLoginBase"/> class.</summary>
        protected AuthenticationLoginBase()
        {
        }

        public event AsyncEventHandler<AuthenticationLoginBase, UserAuthenticatedEventArgs>? UserAuthenticated;

        /// <summary>Gets a value indicating whether the control is Enabled.</summary>
        /// <remarks>This property must be overriden in the inherited class.</remarks>
        public abstract bool Enabled { get; }

        /// <summary>Gets a value indicating whether the control supports Registration.</summary>
        /// <remarks>This property may be overriden in the inherited class.</remarks>
        public virtual bool SupportsRegistration => false;

        /// <summary>Gets or sets the Type of Authentication associated with this control.</summary>
        public string? AuthenticationType { get; set; }

        /// <summary>Gets or sets the Authentication mode of the control (Login or Register).</summary>
        /// <remarks>This property may be overriden in the inherited class.</remarks>
        public virtual AuthMode Mode { get; set; } = AuthMode.Login;

        /// <summary>Gets or sets the Redirect Url for this control.</summary>
        public string? RedirectUrl { get; set; }

        protected virtual ValueTask OnUserAuthenticatedAsync(UserAuthenticatedEventArgs ea)
        {
            return UserAuthenticated.InvokeAsync(this, ea);
        }
    }