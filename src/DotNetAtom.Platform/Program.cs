using System;
using DotNetAtom;
using DotNetAtom.DesktopModules.HTML;
using DotNetAtom.EntityFrameworkCore;
using DotNetAtom.Options;
using DotNetAtom.Providers;
using HttpStack.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

new HtmlModule(); // TODO: Remove this line when assembly loading is fixed.

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions<MachineOptions>()
    .Bind(builder.Configuration.GetSection("Machine"));

builder.Services.AddDotNetAtom(atom =>
{
    var connectionString = builder.Configuration.GetConnectionString("SqlServer");

    if (string.IsNullOrEmpty(connectionString))
    {
        throw new InvalidOperationException("Connection string is not found.");
    }

    atom.AddAspNetPasswordHasher();

    atom.AddEntityFrameworkCore(options =>
    {
        options.UseSqlServer(connectionString);
    });
});

builder.Services.AddSingleton<ITabProvider, LoginTabProvider>();

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
