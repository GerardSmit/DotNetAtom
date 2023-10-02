using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DotNetAtom.Entities;
using DotNetAtom.Repositories.DapperAOT;

namespace DotNetAtom.Portals;

internal class PortalRepository : IPortalRepository
{
    private readonly ConnectionFactory _connectionFactory;

    public PortalRepository(ConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<IPortalInfo>> GetPortalsAsync()
    {
        await using var container = _connectionFactory.CreateConnection();
        var connection = container.SqlConnection;

        var settings = await connection.QueryUnbufferedAsync<PortalSetting>(
                """
                SELECT *
                FROM PortalSettings
                """)
            .GroupBy(e => e.PortalId)
            .ToDictionaryAwaitAsync(e => new ValueTask<int>(e.Key), e => e.ToArrayAsync());

        var users = await connection.QueryUnbufferedAsync<User>(
                """
                SELECT *
                FROM Users
                WHERE UserID IN (
                    SELECT AdministratorID
                    FROM Portals
                )
                """)
            .ToDictionaryAsync(e => e.Id);

        var roles = await connection.QueryUnbufferedAsync<Role>(
                """
                SELECT *
                FROM Roles
                WHERE RoleID IN (
                    SELECT AdministratorRoleID
                    FROM Portals
                )
                OR RoleID IN (
                    SELECT RegisteredRoleID
                    FROM Portals
                )
                """)
            .ToDictionaryAsync(e => e.Id);

        var portals = await connection.QueryUnbufferedAsync<Portal>(
                """
                SELECT *
                FROM Portals
                """)
            .ToDictionaryAsync(e => e.Id, e => e);

        var portalLocalizations = await connection.QueryUnbufferedAsync<PortalLocalization>(
                """
                SELECT *
                FROM PortalLocalization
                """)
            .ToListAsync();

        var result = new List<IPortalInfo>();

        foreach (var portalLocalization in portalLocalizations)
        {
            if (!portals.TryGetValue(portalLocalization.PortalId, out var portal))
            {
                continue;
            }

            var administrator = portal.AdministratorId.HasValue && users.TryGetValue(portal.AdministratorId.Value, out var administratorUsers)
                ? administratorUsers
                : null;

            var administratorRole = portal.AdministratorRoleId.HasValue && roles.TryGetValue(portal.AdministratorRoleId.Value, out var administratorRoles)
                ? administratorRoles
                : null;

            var registeredRole = portal.RegisteredRoleId.HasValue &&
                                 roles.TryGetValue(portal.RegisteredRoleId.Value, out var registeredRoles)
                ? registeredRoles
                : null;

            var portalSettings = settings.TryGetValue(portal.Id, out var portalSettingsArray)
                ? portalSettingsArray
                : Array.Empty<PortalSetting>();

            result.Add(PortalInfo.FromEntity(
                portalLocalization,
                portal,
                administrator,
                administratorRole,
                registeredRole,
                portalSettings));
        }

        return result;
    }
}
