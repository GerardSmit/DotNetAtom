using System;
using DotNetAtom.Application;
using DotNetAtom.Database;
using DotNetAtom.EntityFrameworkCore.Repositories.Applications;
using DotNetAtom.EntityFrameworkCore.Repositories.Tabs;
using DotNetAtom.Infrastructure.EntityFrameworkCore;
using DotNetAtom.Modules;
using DotNetAtom.Portals;
using DotNetAtom.Repositories.EntityFrameworkCore.Repositories.Security;
using DotNetAtom.Security;
using DotNetAtom.Tabs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetAtom.EntityFrameworkCore;

public static class BuilderExtensions
{
    public static AtomBuilder AddEntityFrameworkCore(
        this AtomBuilder builder,
        Action<DbContextOptionsBuilder>? optionsAction = null)
    {
        return builder.AddEntityFrameworkCore<DefaultDbContext>(optionsAction);
    }

    public static AtomBuilder AddEntityFrameworkCore<TContext>(
        this AtomBuilder builder,
        Action<DbContextOptionsBuilder>? optionsAction = null)
        where TContext : AtomDbContext
    {
        builder.Services.AddSingleton<IPortalRepository, PortalRepository>();
        builder.Services.AddSingleton<IModuleRepository, ModuleRepository>();
        builder.Services.AddSingleton<IApplicationRepository, ApplicationRepository>();
        builder.Services.AddSingleton<ITabRepository, TabRepository>();
        builder.Services.AddSingleton<IAuthenticationRepository, AuthenticationRepository>();

        builder.Services.AddDbContextFactory<TContext>(b =>
        {
            optionsAction?.Invoke(b);
        });

        builder.Services.AddSingleton<IDbContextFactory<DbContext>, DbContextFactoryWrapper<TContext, DbContext>>();
        builder.Services.AddSingleton<IDbContextFactory<AtomDbContext>, DbContextFactoryWrapper<TContext, AtomDbContext>>();
        builder.Services.AddScoped<TContext>(provider => provider.GetRequiredService<IDbContextFactory<TContext>>().CreateDbContext());
        builder.Services.AddScoped<AtomDbContext>(provider => provider.GetRequiredService<TContext>());
        builder.Services.AddScoped<DbContext>(provider => provider.GetRequiredService<TContext>());

        return builder;
    }
}
