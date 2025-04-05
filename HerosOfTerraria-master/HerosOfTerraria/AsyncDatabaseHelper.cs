using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TShockAPI;
using TShockAPI.DB;

/// <summary>
/// Provides asynchronous database operations for TShock plugins,
/// including query execution, scalar retrieval, schema management, and row mapping.
/// </summary>
public class AsyncDatabaseHelper
{
    private readonly IDbConnection _db;

    /// <summary>
    /// Initializes a new instance of the <see cref="AsyncDatabaseHelper"/> class.
    /// </summary>
    /// <param name="dbConnection">The TShock database connection instance (usually TShock.DB).</param>
    public AsyncDatabaseHelper(IDbConnection dbConnection)
    {
        _db = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
    }

    /// <summary>
    /// Asynchronously executes a non-query command such as INSERT, UPDATE, or DELETE.
    /// </summary>
    public async Task ExecuteNonQueryAsync(string sql, params object[] args)
    {
        await Task.Run(() =>
        {
            try
            {
                _db.Query(sql, args);
            }
            catch (Exception ex)
            {
                TShock.Log.ConsoleError($"[DB ERROR] NonQuery failed: {ex.Message}\nQuery: {sql}");
            }
        });
    }

    /// <summary>
    /// Asynchronously executes a scalar query and returns a single value.
    /// </summary>
    public async Task<T> ExecuteScalarAsync<T>(string sql, string columnName, params object[] args)
    {
        return await Task.Run(() =>
        {
            try
            {
                using (var reader = _db.QueryReader(sql, args))
                {
                    if (reader.Read())
                    {
                        return reader.Get<T>(columnName);
                    }
                }
            }
            catch (Exception ex)
            {
                TShock.Log.ConsoleError($"[DB ERROR] Scalar failed: {ex.Message}\nQuery: {sql}");
            }

            return default;
        });
    }

    /// <summary>
    /// Asynchronously executes a query and returns a list of dictionaries (column name/value pairs).
    /// </summary>
    public async Task<List<Dictionary<string, object>>> ExecuteQueryAsync(string sql, string[] columnNames, params object[] args)
    {
        return await Task.Run(() =>
        {
            var result = new List<Dictionary<string, object>>();

            try
            {
                using (var reader = _db.QueryReader(sql, args))
                {
                    while (reader.Read())
                    {
                        var row = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
                        foreach (var column in columnNames)
                        {
                            row[column] = reader.Get<object>(column);
                        }
                        result.Add(row);
                    }
                }
            }
            catch (Exception ex)
            {
                TShock.Log.ConsoleError($"[DB ERROR] Query failed: {ex.Message}\nQuery: {sql}");
            }

            return result;
        });
    }

    /// <summary>
    /// Asynchronously executes a query and maps each row to a custom object.
    /// </summary>
    public async Task<List<T>> ExecuteQueryAsync<T>(string sql, Func<QueryResult, T> mapper, params object[] args)
    {
        return await Task.Run(() =>
        {
            var result = new List<T>();

            try
            {
                using (var reader = _db.QueryReader(sql, args))
                {
                    while (reader.Read())
                    {
                        result.Add(mapper(reader));
                    }
                }
            }
            catch (Exception ex)
            {
                TShock.Log.ConsoleError($"[DB ERROR] Mapping query failed: {ex.Message}\nQuery: {sql}");
            }

            return result;
        });
    }

    /// <summary>
    /// Asynchronously ensures a table or schema exists by executing a DDL command.
    /// </summary>
    public async Task EnsureTableAsync(string ddlSql)
    {
        await Task.Run(() =>
        {
            try
            {
                _db.Query(ddlSql);
            }
            catch (Exception ex)
            {
                TShock.Log.ConsoleError($"[DB ERROR] Table creation failed: {ex.Message}\nDDL: {ddlSql}");
            }
        });
    }

    /// <summary>
    /// Asynchronously checks if any rows exist for the specified query.
    /// </summary>
    public async Task<bool> RowExistsAsync(string sql, params object[] args)
    {
        return await Task.Run(() =>
        {
            try
            {
                using (var reader = _db.QueryReader(sql, args))
                {
                    return reader.Read();
                }
            }
            catch (Exception ex)
            {
                TShock.Log.ConsoleError($"[DB ERROR] RowExists failed: {ex.Message}\nQuery: {sql}");
                return false;
            }
        });
    }

}
