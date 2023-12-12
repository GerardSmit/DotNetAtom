using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using DotNetAtom.Framework;
using DotNetAtom.Portals;
using DotNetAtom.Security;
using DotNetAtom.Services.Authentication;
using DotNetAtom.Sessions;
using Microsoft.Extensions.Logging;
using WebFormsCore.UI.HtmlControls;

namespace DotNetAtom.DesktopModules.Admin.Authentication;

public partial class Login(
    IAuthenticationService authenticationService,
    IUserSessionService userSessionService,
    ILogger<Login> logger
) : PortalModuleBase
{
    private readonly List<AuthenticationLoginBase> _additionalLoginControls = new();
    private readonly List<OAuthLoginBase> _oAuthControls = new();

    public string? RedirectUrl { get; set; }

    protected override async ValueTask OnInitAsync(CancellationToken token)
    {
        await base.OnInitAsync(token);

        if (User.UserId == -1)
        {
            await LoadProvidersAsync();
        }
        else if (Tab.TabSettings.ContainsKey("_IsLoginTab") || Tab.TabId == PortalSettings.Portal.LoginTabId)
        {
            Redirect();
        }
    }

    private async ValueTask LoadProvidersAsync()
    {
        AuthenticationLoginBase? defaultLoginControl = null;
        var defaultAuthProvider = PortalSettings.Portal.GetPortalSetting("DefaultAuthProvider", "DNN");

        // Load all the login controls
        foreach (var authSystem in authenticationService.EnabledAuthentications)
        {
            var authLoginControl = (AuthenticationLoginBase)LoadControl(authSystem.LoginControlSrc);

            BindLoginControl(authLoginControl, authSystem.LoginControlSrc, authSystem.AuthenticationType);

            if (defaultLoginControl is null && authSystem.AuthenticationType == "DNN")
            {
                // Remember the default DNN login control if the default is not set yet
                // This will be used if no other controls are enabled for the portal
                defaultLoginControl = authLoginControl;
            }

            if (!authLoginControl.Enabled)
            {
                continue;
            }

            if (authLoginControl is OAuthLoginBase oAuthLoginControl)
            {
                _oAuthControls.Add(oAuthLoginControl);
            }
            else if (authLoginControl.AuthenticationType == defaultAuthProvider)
            {
                defaultLoginControl = authLoginControl;
            }
            else
            {
                _additionalLoginControls.Add(authLoginControl);
            }
        }

        if (defaultLoginControl == null)
        {
            // No controls enabled for portal, and default DNN control is not enabled by host, so load system default (DNN)
            var authSystem = authenticationService.GetAuthenticationSystem("DNN");

            AuthenticationLoginBase authLoginControl;

            if (authSystem is null)
            {
                // The default DNN authentication system is not installed in the database
                // Fall back to the known path of the DNN login control
                const string defaultPath = "DesktopModules/AuthenticationServices/DNN/Login.ascx";

                logger.LogWarning("The DNN authentication system is not installed in the database. Falling back to the known path of the DNN login control.");

                authLoginControl = (AuthenticationLoginBase)LoadControl(defaultPath);
                BindLoginControl(authLoginControl, defaultPath, "DNN");
            }
            else
            {
                authLoginControl = (AuthenticationLoginBase)LoadControl(authSystem.LoginControlSrc);
                BindLoginControl(authLoginControl, authSystem.LoginControlSrc, authSystem.AuthenticationType);
            }

            defaultLoginControl = authLoginControl;
        }

        await DisplayLoginControl(defaultLoginControl);

        if (_additionalLoginControls.Count > 0)
        {
            // TODO: Display additional login controls
        }
    }

    private async ValueTask DisplayLoginControl(AuthenticationLoginBase authLoginControl)
    {
        var container = new HtmlGenericControl
        {
            TagName = "div",
            ID = authLoginControl.AuthenticationType
        };

        await container.Controls.AddAsync(authLoginControl);

        await pnlLoginContainer.Controls.AddAsync(container);

        pnlLoginContainer.Visible = true;
    }

    private void BindLoginControl(AuthenticationLoginBase authLoginControl, string loginSrc, string type)
    {
        var name = Path.GetFileName(loginSrc);

        authLoginControl.AuthenticationType = type;
        authLoginControl.ID = $"{name}_{type}";
        authLoginControl.LocalResourceFile = $"{authLoginControl.TemplateSourceDirectory}/App_LocalResources/{name}.resx";
        authLoginControl.RedirectUrl = RedirectUrl;

        authLoginControl.UserAuthenticated += UserAuthenticated;
    }

    private async Task UserAuthenticated(AuthenticationLoginBase sender, UserAuthenticatedEventArgs e)
    {
        await userSessionService.SetCurrentUserAsync(e.User);
        Redirect();
    }

    private void Redirect()
    {
        Response.StatusCode = 302;
        Response.Headers["Location"] = "/";
    }
}