using System;
using System.Reflection;
using DotNetAtom;
using DotNetAtom.DesktopModules.HTML;
using DotNetAtom.Providers;
using DotNetAtom.Repositories.DapperAOT;
using HttpStack.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

new HtmlModule(); // TODO: Remove this line when assembly loading is fixed.

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDotNetAtom(atom =>
{
    var connectionString = builder.Configuration.GetConnectionString("SqlServer");

    if (string.IsNullOrEmpty(connectionString))
    {
        throw new InvalidOperationException("Connection string is not found.");
    }

    atom.AddDapperAOT(connectionString);
    atom.AddTripleDES("5FDB5CE1F4531470763A34A78398E3D355EFF4EE57426EFB");
});

builder.Services.AddSingleton<ITabProvider, TestTabProvider>();

var app = builder.Build();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(app.Environment.ContentRootPath)
});

app.UseStack(stack =>
{
    stack.UseDotNetAtom();
    stack.UseDotNetAtomPage();
});

app.Run();
