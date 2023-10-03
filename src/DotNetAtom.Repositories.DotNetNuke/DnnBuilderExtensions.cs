using DotNetAtom.Application;
using DotNetAtom.Applications;
using DotNetAtom.Modules;
using DotNetAtom.Portals;
using DotNetAtom.Security;
using DotNetAtom.Tabs;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetAtom;

public static class DnnBuilderExtensions
{
    public static AtomBuilder AddDotNetNuke(this AtomBuilder builder)
    {
        builder.Services.AddSingleton<IPortalRepository, PortalRepository>();
        builder.Services.AddSingleton<IModuleRepository, ModuleRepository>();
        builder.Services.AddSingleton<IApplicationRepository, ApplicationRepository>();
        builder.Services.AddSingleton<ITabRepository, TabRepository>();
        builder.Services.AddSingleton<IAuthenticationRepository, AuthenticationRepository>();

        return builder;
    }
}
