using Dapper;
using DotNetAtom.Entities;
using DotNetAtom.Modules;
using DotNetAtom.Repositories.DapperAOT;
using DotNetAtom.Tabs.Cache;

namespace DotNetAtom.Infrastructure.EntityFrameworkCore;

internal class ModuleRepository : IModuleRepository
{
    private readonly ConnectionFactory _connectionFactory;

    public ModuleRepository(ConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyDictionary<int, IModuleDefinitionInfo>> GetDefinitionsAsync()
    {
        await using var container = _connectionFactory.CreateConnection();
        var connection = container.SqlConnection;

        var moduleDefinitions = await connection.QueryAsync<ModuleDefinition>(
            """
            SELECT *
            FROM ModuleDefinitions
            """);

        var desktopModule = await connection.QueryUnbufferedAsync<DesktopModule>(
            """
            SELECT *
            FROM DesktopModules
            """)
            .ToDictionaryAsync(m => m.Id);

        var moduleControls = await connection.QueryUnbufferedAsync<ModuleControl>(
            """
            SELECT *
            FROM ModuleControls
            WHERE ModuleDefID IS NOT NULL
            """)
            .GroupBy(mc => mc.ModuleDefinitionId!.Value)
            .ToDictionaryAwaitAsync(g => new ValueTask<int>(g.Key), g => g.ToArrayAsync());

        var result = new Dictionary<int, IModuleDefinitionInfo>();

        foreach (var moduleDefinition in moduleDefinitions)
        {
            if (!desktopModule.TryGetValue(moduleDefinition.DesktopModuleId, out var desktopModuleInfo))
            {
                continue;
            }

            if (!moduleControls.TryGetValue(moduleDefinition.Id, out var controls))
            {
                controls = Array.Empty<ModuleControl>();
            }

            result.Add(moduleDefinition.Id, new ModuleDefinitionInfo(moduleDefinition, desktopModuleInfo, controls));
        }

        return result;
    }
}
