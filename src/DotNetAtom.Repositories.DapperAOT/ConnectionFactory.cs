using Microsoft.Data.SqlClient;

namespace DotNetAtom.Repositories.DapperAOT;

internal class ConnectionFactory
{
    private readonly string _connectionString;

    public ConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public ConnectionContainer CreateConnection()
    {
        return new ConnectionContainer(new SqlConnection(_connectionString));
    }
}