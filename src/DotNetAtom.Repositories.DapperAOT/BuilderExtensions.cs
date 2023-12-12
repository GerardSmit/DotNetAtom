#pragma warning disable IL2026
#pragma warning disable IL2072

using DotNetAtom.Application;
using DotNetAtom.EntityFrameworkCore.Repositories.Applications;
using DotNetAtom.EntityFrameworkCore.Repositories.Tabs;
using DotNetAtom.Infrastructure.EntityFrameworkCore;
using DotNetAtom.Modules;
using DotNetAtom.Portals;
using DotNetAtom.Repositories.DapperAOT.Repositories.Security;
using DotNetAtom.Repositories.DapperAOT.Repositories.Users;
using DotNetAtom.Security;
using DotNetAtom.Tabs;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetAtom.Repositories.DapperAOT;

public static class BuilderExtensions
{
    public static AtomBuilder AddDapperAOT(this AtomBuilder builder, string connectionString)
    {
        builder.Services.AddSingleton(new ConnectionFactory(connectionString));
        builder.Services.AddSingleton<IPortalRepository, PortalRepository>();
        builder.Services.AddSingleton<IModuleRepository, ModuleRepository>();
        builder.Services.AddSingleton<IApplicationRepository, ApplicationRepository>();
        builder.Services.AddSingleton<ITabRepository, TabRepository>();
        builder.Services.AddSingleton<IAuthenticationRepository, AuthenticationRepository>();
        builder.Services.AddSingleton<IAspNetUserRepository, AspNetUserRepository>();
        builder.Services.AddSingleton<IUserRepository, UserRepository>();

        return builder;
    }
}