using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using DotNetAtom.Framework;
using DotNetAtom.Middlewares;
using DotNetAtom.Modules;
using DotNetAtom.Modules.Factories;
using DotNetAtom.Providers;
using DotNetAtom.Sessions;
using HttpStack;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WebFormsCore;
using WebFormsCore.UI;
using WebFormsCore.UI.WebControls;

namespace DotNetAtom;

internal class EmptyControlFactory : IControlFactory<Control>
{
    public Control CreateControl(IServiceProvider provider)
    {
        return new ControlHolder();
    }

    object IControlFactory.CreateControl(IServiceProvider provider) => CreateControl(provider);
}

internal class ControlHolder : Control, INamingContainer
{
}

internal class EmptyFallback(
    IServiceProvider serviceProvider,
    ILogger<EmptyFallback> logger
) : IControlManager
{
    private readonly DefaultControlManager _controlManagerImplementation = ActivatorUtilities.CreateInstance<DefaultControlManager>(serviceProvider);

    public Type GetType(string path)
    {
        try
        {
            return _controlManagerImplementation.GetType(path);
        }
        catch (FileNotFoundException)
        {
            logger.LogWarning("The control {Path} was not found.", path);
            return typeof(Control);
        }
    }

    public ValueTask<Type> GetTypeAsync(string path)
    {
        try
        {
            return _controlManagerImplementation.GetTypeAsync(path);
        }
        catch (FileNotFoundException)
        {
            logger.LogWarning("The control {Path} was not found.", path);
            return new ValueTask<Type>(typeof(Control));
        }
    }

    public bool TryGetPath(string fullPath, [NotNullWhen(true)] out string? path)
    {
        return _controlManagerImplementation.TryGetPath(fullPath, out path);
    }

    public IReadOnlyDictionary<string, Type> ViewTypes => _controlManagerImplementation.ViewTypes;
}

public static class WebServiceExtensions
{
    public static IServiceCollection AddDotNetAtom(this IServiceCollection services, Action<AtomBuilder>? configure = null)
    {
        services.TryAddSingleton<IHostEnvironment, DefaultHostEnvironment>();
        services.TryAddScoped<IHttpContextProvider, DefaultHttpContextProvider>();
        services.TryAddScoped<IUserSessionService, CookieUserSessionService>();

        services.AddSingleton<IModuleControlFactory, WebFormsControlFactory>();
        services.AddSingleton<IModuleControlService, ModuleControlService>();

        services.AddSingleton<AtomMiddleware>();

        services.AddDotNetAtomCore(configure);
        services.AddSingleton<IControlManager, EmptyFallback>();
        services.AddSingleton<IControlFactory<Control>, EmptyControlFactory>();
        services.AddWebForms(b =>
        {
            b.TryAddEnumAttributeParser<TextBoxMode>();

            b.AddClientResourceManagement(c =>
            {
                c.RegisterPrefix("SkinPath", i => i.Context.Features.Get<IAtomFeature>()?.AtomContext.PortalSettings?.CurrentSkinPath ?? string.Empty);
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
