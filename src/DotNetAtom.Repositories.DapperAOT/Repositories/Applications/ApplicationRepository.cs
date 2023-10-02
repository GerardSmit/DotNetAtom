using System;
using System.Threading.Tasks;
using Dapper;
using DotNetAtom.Application;
using DotNetAtom.Entities;
using DotNetAtom.Repositories.DapperAOT;

namespace DotNetAtom.EntityFrameworkCore.Repositories.Applications;

internal class ApplicationRepository : IApplicationRepository
{
    private readonly ConnectionFactory _connectionFactory;

    public ApplicationRepository(ConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Guid?> GetApplicationId()
    {
        await using var container = _connectionFactory.CreateConnection();
        var connection = container.SqlConnection;

        var guid = await connection.QueryFirstOrDefaultAsync<Guid?>(
            """
            SELECT ApplicationId
            FROM aspnet_Applications
            WHERE LoweredApplicationName = 'dotnetnuke'
            """);

        return guid;
    }
}
