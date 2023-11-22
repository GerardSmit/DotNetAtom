<%@ Control Inherits="DotNetAtom.DesktopModules.AuthenticationServices.DNN.Login" %>
<%@ Register TagPrefix="asp" Namespace="WebFormsCore.UI.WebControls" %>
<%@ Register TagPrefix="asp" Namespace="WebFormsCore.UI.HtmlControls" %>

<asp:HtmlForm runat="server" OnSubmit="OnSubmit">
  <div class="form-group">
    <asp:Label runat="server" ID="lblUsername" AssociatedControlID="tbUsername" />
    <asp:TextBox runat="server" class="form-control" id="tbUsername" ValidationGroup="Login" />
    <asp:RequiredFieldValidator runat="server" ID="valUsername" ControlToValidate="tbUsername" ValidationGroup="Login" />
  </div>

  <div class="form-group">
    <asp:Label runat="server" ID="lblPassword" AssociatedControlID="tbPassword" />
    <asp:TextBox runat="server" class="form-control" id="tbPassword" TextMode="Password" ValidationGroup="Login" />
    <asp:RequiredFieldValidator runat="server" ID="valPassword" ControlToValidate="tbPassword" ValidationGroup="Login" />
  </div>

  <asp:Button runat="server" class="btn btn-primary" ID="btnSignIn" ValidationGroup="Login" />
</asp:HtmlForm>