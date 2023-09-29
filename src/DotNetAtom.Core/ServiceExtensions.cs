using System;
using DotNetAtom.Application;
using DotNetAtom.Modules;
using DotNetAtom.Modules.Factories;
using DotNetAtom.Portals;
using DotNetAtom.Skins;
using DotNetAtom.Tabs;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetAtom;

public static class ServiceExtensions
{
    public static IServiceCollection AddDotNetAtomCore(this IServiceCollection services, Action<AtomBuilder>? configure = null)
    {
        var builder = new AtomBuilder(services);
        configure?.Invoke(builder);

        services.AddHostedService<InitializeService>();

        services.AddSingleton<IAtomGlobals, AtomGlobals>();

        // Control factories
        services.AddSingleton<IModuleControlFactory, WebFormsControlFactory>();
        services.AddSingleton<IModuleControlService, ModuleControlService>();

        // Services
        services.AddSingleton<IPortalService, PortalService>();
        services.AddSingleton<ITabService, TabService>();
        services.AddSingleton<ITabRouter, TabRouter>();
        services.AddSingleton<ISkinService, SkinService>();
        services.AddSingleton<IModuleService, ModuleService>();

        // Application
        services.AddSingleton<IAtomContextFactory, AtomContextFactory>();

        return services;
    }
}
