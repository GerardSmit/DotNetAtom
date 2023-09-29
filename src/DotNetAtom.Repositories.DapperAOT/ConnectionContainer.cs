using Microsoft.Data.SqlClient;

namespace DotNetAtom.Repositories.DapperAOT;

public readonly struct ConnectionContainer : IAsyncDisposable
{
    public ConnectionContainer(SqlConnection connection)
    {
        SqlConnection = connection;
    }

    public SqlConnection SqlConnection { get; }

    public ValueTask DisposeAsync()
    {
#if NETSTANDARD2_0
        SqlConnection.Dispose();
        return default;
#else
        return SqlConnection.DisposeAsync();
#endif
    }
}
