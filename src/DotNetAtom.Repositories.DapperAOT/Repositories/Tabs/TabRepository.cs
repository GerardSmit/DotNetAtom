using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DotNetAtom.Entities;
using DotNetAtom.Repositories.DapperAOT;
using DotNetAtom.Tabs;
using DotNetAtom.Tabs.Cache;

namespace DotNetAtom.EntityFrameworkCore.Repositories.Tabs;

internal class TabRepository : ITabRepository
{
    private readonly ConnectionFactory _connectionFactory;

    public TabRepository(ConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<ITabInfo>> GetTabsAsync()
    {
        await using var container = _connectionFactory.CreateConnection();
        var connection = container.SqlConnection;

        var tabModules = await connection.QueryUnbufferedAsync<TabModule>(
                """
                SELECT *
                FROM TabModules
                """)
            .GroupBy(i => i.TabId)
            .ToDictionaryAwaitAsync(m => new ValueTask<int>(m.Key), m => m.ToArrayAsync());

        var modules = await connection.QueryUnbufferedAsync<Module>(
                """
                SELECT *
                FROM Modules
                """)
            .ToDictionaryAsync(m => m.Id);

        var texts = await connection.QueryUnbufferedAsync<HtmlText>(
                """
                SELECT *
                FROM HtmlText
                """)
            .GroupBy(i => i.ModuleId)
            .ToDictionaryAwaitAsync(m => new ValueTask<int>(m.Key), m => m.ToArrayAsync());

        var settings = await connection.QueryUnbufferedAsync<TabModuleSetting>(
                """
                SELECT *
                FROM TabModuleSettings
                """)
            .GroupBy(i => i.TabModuleId)
            .ToDictionaryAwaitAsync(m => new ValueTask<int>(m.Key), m => m.ToArrayAsync());

        var permissions = await connection.QueryUnbufferedAsync<ModulePermission>(
                """
                SELECT *
                FROM ModulePermission
                """)
            .GroupBy(i => i.ModuleId)
            .ToDictionaryAwaitAsync(m => new ValueTask<int>(m.Key), m => m.ToArrayAsync());

        var tabs = await connection.QueryAsync<Tab>(
                """
                SELECT *
                FROM Tabs
                """);

        var tabPermissions = await connection.QueryUnbufferedAsync<TabPermission>(
                """
                SELECT *
                FROM TabPermission
                """)
            .GroupBy(i => i.TabId)
            .ToDictionaryAwaitAsync(m => new ValueTask<int>(m.Key), m => m.ToArrayAsync());

        var tabSettings = await connection.QueryUnbufferedAsync<TabSetting>(
                """
                SELECT *
                FROM TabSettings
                """)
            .GroupBy(i => i.TabId)
            .ToDictionaryAwaitAsync(m => new ValueTask<int>(m.Key), m => m.ToArrayAsync());

        var result = new List<ITabInfo>();

        foreach (var tab in tabs)
        {
            var moduleInfos = new List<ModuleInfo>();

            if (tabModules.TryGetValue(tab.Id, out var currentTabModules))
            {
                foreach (var tabModule in currentTabModules)
                {
                    if (!modules.TryGetValue(tabModule.ModuleId, out var module))
                    {
                        continue;
                    }

                    if (!texts.TryGetValue(tabModule.ModuleId, out var currentTexts))
                    {
                        currentTexts = Array.Empty<HtmlText>();
                    }

                    if (!settings.TryGetValue(tabModule.Id, out var currentSettings))
                    {
                        currentSettings = Array.Empty<TabModuleSetting>();
                    }

                    if (!permissions.TryGetValue(tabModule.ModuleId, out var currentPermissions))
                    {
                        currentPermissions = Array.Empty<ModulePermission>();
                    }

                    var moduleInfo = new ModuleInfo(
                        tabModule,
                        module,
                        currentTexts,
                        currentSettings,
                        currentPermissions
                    );

                    moduleInfos.Add(moduleInfo);
                }
            }

            if (!tabPermissions.TryGetValue(tab.Id, out var currentTabPermissions))
            {
                currentTabPermissions = Array.Empty<TabPermission>();
            }

            if (!tabSettings.TryGetValue(tab.Id, out var currentTabSettings))
            {
                currentTabSettings = Array.Empty<TabSetting>();
            }

            var tabInfo = new TabInfo(
                tab,
                moduleInfos,
                currentTabPermissions,
                currentTabSettings
            );

            result.Add(tabInfo);
        }

        return result;
    }
}
