using System;
using System.Reflection;
using System.Threading.Tasks;
using DotNetAtom.Framework;
using DotNetAtom.Middlewares;
using HttpStack;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using WebFormsCore;

namespace DotNetAtom;

public static class WebServiceExtensions
{
    public static IServiceCollection AddDotNetAtom(this IServiceCollection services, Action<AtomBuilder>? configure = null)
    {
        services.TryAddSingleton<IHostEnvironment, DefaultHostEnvironment>();

        services.AddSingleton<AtomMiddleware>();

        services.AddDotNetAtomCore(configure);
        services.AddWebForms(b =>
        {
            b.AddClientResourceManagement(c =>
            {
                c.RegisterPrefix("SkinPath", _ => "/Portals/_default/Skins/Xcillion/");
                c.RegisterPrefix("SharedScripts", "/Resources/Shared/Scripts/");
            });
        });

        return services;
    }

    public static IHttpStackBuilder UseDotNetAtom(this IHttpStackBuilder app)
    {
        app.UseWebFormsCore();

        var middleware = ActivatorUtilities.GetServiceOrCreateInstance<AtomMiddleware>(app.Services);
        return app.UseMiddleware(middleware);
    }

    public static IHttpStackBuilder UseDotNetAtomPage(this IHttpStackBuilder app, bool handleNotFound = true)
    {
        var middleware = ActivatorUtilities.GetServiceOrCreateInstance<AtomPageMiddleware>(app.Services);
        app.UseMiddleware(middleware);

        if (handleNotFound)
        {
            app.Run(context =>
            {
                context.Response.StatusCode = 404;
                return Task.CompletedTask;
            });
        }

        return app;
    }
}
