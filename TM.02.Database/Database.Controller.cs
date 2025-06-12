namespace TaskManager.Database;

using Microsoft.Data.Sqlite;
using Dapper;

public class Controller<T>
{
    internal static string DbConnectionString => "database.db";

    public async Task<IEnumerable<T>> Select(string queryString)
    {
        using SqliteConnection db = new SqliteConnection(DbConnectionString);
        var result = await db.QueryAsync<T>(queryString);
        return result;
    }

    public async Task InsertMultiple(string queryString, List<T> values)
    {
        using SqliteConnection db = new SqliteConnection(DbConnectionString);
        db.Open();
        await db.ExecuteAsync(queryString, values);
        db.Close();
    }

    public async Task InsertSingle(string queryString, T value)
    {
        using SqliteConnection db = new SqliteConnection(DbConnectionString);
        db.Open();
        await db.ExecuteAsync(queryString, value);
        db.Close();
    }

    public async Task UpdateSingle(string queryString, T value)
    {
        using SqliteConnection db = new SqliteConnection(DbConnectionString);
        db.Open();
        await db.ExecuteAsync(queryString, value);
        db.Close();
    }

    public async Task DeleteSingle(string queryString, int id)
    {
        using SqliteConnection db = new SqliteConnection(DbConnectionString);
        db.Open();
        await db.ExecuteAsync(queryString, id);
        db.Close();
    }

}
