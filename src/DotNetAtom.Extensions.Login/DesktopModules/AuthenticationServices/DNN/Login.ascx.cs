using DotNetAtom.Services.Authentication;

namespace DotNetAtom.DesktopModules.AuthenticationServices.DNN;

public partial class Login : AuthenticationLoginBase
{
    public override bool Enabled => true;
}
