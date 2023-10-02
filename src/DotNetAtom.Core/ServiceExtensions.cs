using System;
using DotNetAtom.Application;
using DotNetAtom.Modules;
using DotNetAtom.Modules.Factories;
using DotNetAtom.Portals;
using DotNetAtom.Providers;
using DotNetAtom.Skins;
using DotNetAtom.Tabs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DotNetAtom;

public static class ServiceExtensions
{
    public static IServiceCollection AddDotNetAtomCore(this IServiceCollection services, Action<AtomBuilder>? configure = null)
    {
        var builder = new AtomBuilder(services);
        configure?.Invoke(builder);

        services.AddHostedService<InitializeService>();

        services.TryAddSingleton<IAtomGlobals, AtomGlobals>();

        // Control factories
        services.TryAddSingleton<IModuleControlFactory, WebFormsControlFactory>();
        services.TryAddSingleton<IModuleControlService, ModuleControlService>();

        // Default providers
        services.TryAddSingleton<ITabTitleProvider, TabTitleProvider>();
        services.TryAddSingleton<ITabProvider, DatabaseTabProvider>();

        // Services
        services.TryAddSingleton<IPortalService, PortalService>();
        services.TryAddSingleton<ITabService, TabService>();
        services.TryAddSingleton<ITabRouter, TabRouter>();
        services.TryAddSingleton<ISkinService, SkinService>();
        services.TryAddSingleton<IModuleService, ModuleService>();

        // Application
        services.TryAddSingleton<IAtomContextFactory, AtomContextFactory>();

        return services;
    }
}
