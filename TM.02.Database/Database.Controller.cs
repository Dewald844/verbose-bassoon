namespace TaskManager.Database;

using Microsoft.Data.Sqlite;
using Dapper;
using System.Data;

public class Database<T>
{
    internal static string DbConnectionString => "database.db";

    public async Task<IEnumerable<T>> Select (string queryString)
    {
        using IDbConnection db = new SqliteConnection(DbConnectionString);
        var result = await db.QueryAsync<T>(queryString);
        return result;
    }

    public async Task Insert (string queryString, List<T> values)
    {
        using IDbConnection db = new SqliteConnection(DbConnectionString);
        db.Open();
        await db.ExecuteAsync(queryString, values);
        db.Close();
    }

    public async Task UpdateSingle (string queryString, T value)
    {
        using IDbConnection db = new SqliteConnection(DbConnectionString);
        db.Open();
        await db.ExecuteAsync(queryString, value);
        db.Close();
    }

}
