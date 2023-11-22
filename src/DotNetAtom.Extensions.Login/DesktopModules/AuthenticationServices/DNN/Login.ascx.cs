using System;
using System.Threading.Tasks;
using DotNetAtom.Framework;
using DotNetAtom.Security;
using DotNetAtom.Security.Membership;
using DotNetAtom.Services.Authentication;
using DotNetAtom.Sessions;
using WebFormsCore.UI.HtmlControls;
using WebFormsCore.UI.WebControls;

namespace DotNetAtom.DesktopModules.AuthenticationServices.DNN;

public partial class Login(
    IUserService userService
) : AuthenticationLoginBase
{
    public override bool Enabled => true;

    protected override void OnInit(EventArgs args)
    {
        base.OnInit(args);

        lblUsername.Text = GetString("lblUsername");
        lblPassword.Text = GetString("lblPassword");
        btnSignIn.Text = GetString("btnSignIn");
        valUsername.Text = GetString("valUsername.ErrorMessage");
        valPassword.Text = GetString("valPassword.ErrorMessage");
    }

    protected async Task OnSubmit(HtmlForm sender, EventArgs e)
    {
        var applicationId = AtomContext.ApplicationId;
        var result = await userService.AuthenticateAsync(applicationId, PortalId, tbUsername.Text, tbPassword.Text);

        if (result.IsSuccess)
        {
            await OnUserAuthenticatedAsync(new UserAuthenticatedEventArgs(result.UserInfo));
            return;
        }

        await ModuleContext.AddModuleMessageAsync(GetString($"Error.{result.Status}"), ModuleMessageType.RedError, true);
    }
}
