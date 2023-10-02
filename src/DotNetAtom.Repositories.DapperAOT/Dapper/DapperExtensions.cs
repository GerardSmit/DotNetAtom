#if NETSTANDARD2_0
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq.Expressions;
using System.Reflection;
using Dapper;

namespace DotNetAtom.Repositories.DapperAOT;

public static partial class DapperExtensions
{
    public static async IAsyncEnumerable<T> QueryUnbufferedAsync<T>(this DbConnection connection, string sql)
    {
        using var reader = await connection.ExecuteReaderAsync(sql);
        var parser = reader.GetRowParser<T>();

        while (await reader.ReadAsync())
        {
            yield return parser(reader);
        }
    }

    public static async IAsyncEnumerable<T> ReadUnbufferedAsync<T>(this SqlMapper.GridReader connection)
    {
        var reader = GetReader(connection);
        var parser = reader.GetRowParser<T>();

        while (await reader.ReadAsync())
        {
            yield return parser(reader);
        }
    }

    private static Func<SqlMapper.GridReader, DbDataReader> GetReader;

    static DapperExtensions()
    {
        var field = typeof(SqlMapper.GridReader).GetField("reader", BindingFlags.NonPublic | BindingFlags.Instance);
        var parameter = Expression.Parameter(typeof(SqlMapper.GridReader));
        var body = Expression.Field(parameter, field);
        var lambda = Expression.Lambda<Func<SqlMapper.GridReader, DbDataReader>>(body, parameter);
        GetReader = lambda.Compile();
    }
}
#endif