﻿using System;
using System.Threading;
using System.Threading.Tasks;
using DotNetAtom.UI.Skins;
using WebFormsCore.UI;

namespace DotNetAtom.UI.Skins.Controls;

public partial class Logo : SkinObjectBase
{
    public override async ValueTask RenderAsync(HtmlTextWriter writer, CancellationToken token)
    {
        await writer.WriteAsync("<a href=\"");
        await writer.WriteAsync("/"); // TODO: Resolve home page
        await writer.WriteAsync("\">");

        await writer.WriteAsync("<img src=\"/Portals/0/");
        await writer.WriteAsync(PortalSettings.LogoFile);
        await writer.WriteAsync("\" alt=\"");
        await writer.WriteAsync(PortalSettings.PortalName);
        await writer.WriteAsync("\" />");

        await writer.WriteAsync("</a>");
    }
}
