using System;

namespace WhaTap.Trace.Utils;

internal static class DbType
{
    public const string MySql = "mysql";

    public const string PostgreSql = "postgres";

    public const string Oracle = "oracle";

    public const string SqlServer = "sql-server";

    public const string Sqlite = "sqlite";
}

public static class DbUtils
{
    public static bool TryGetIntegrationDetails(string? commandTypeFullName,out string dbType)
    {
        if (string.IsNullOrEmpty(commandTypeFullName))
        {
            dbType = string.Empty;
            return false;
        }

        switch (commandTypeFullName)
        {
            case "System.Data.SqlClient.SqlCommand" or "Microsoft.Data.SqlClient.SqlCommand":
                dbType = DbType.SqlServer;
                return true;
            case "Npgsql.NpgsqlCommand":
                dbType = DbType.PostgreSql;
                return true;
            case "MySql.Data.MySqlClient.MySqlCommand" or "MySqlConnector.MySqlCommand":
                dbType = DbType.MySql;
                return true;
            case "Oracle.ManagedDataAccess.Client.OracleCommand" or "Oracle.DataAccess.Client.OracleCommand":
                dbType = DbType.Oracle;
                return true;
            case "System.Data.SQLite.SQLiteCommand" or "Microsoft.Data.Sqlite.SqliteCommand":
                dbType = DbType.Sqlite;
                return true;
            default:
                string commandTypeName = commandTypeFullName.Substring(commandTypeFullName.LastIndexOf(".") + 1);
                if (commandTypeName == "InterceptableDbCommand" || commandTypeName == "ProfiledDbCommand")
                {
                    dbType = string.Empty;
                    return false;
                }

                const string commandSuffix = "Command";
                int lastIndex = commandTypeFullName.LastIndexOf(".");
                string namespaceName = lastIndex > 0 ? commandTypeFullName.Substring(0, lastIndex) : string.Empty;
                dbType = commandTypeName switch
                {
                    _ when namespaceName.Length == 0 && commandTypeName == commandSuffix => "command",
                    _ when namespaceName.Contains(".") && commandTypeName == commandSuffix =>
                        // the + 1 could be dangerous and cause IndexOutOfRangeException, but this shouldn't happen
                        // a period should never be the last character in a namespace
                        namespaceName.Substring(namespaceName.LastIndexOf('.') + 1).ToLowerInvariant(),
                    _ when commandTypeName == commandSuffix =>
                        namespaceName.ToLowerInvariant(),
                    _ when commandTypeName.EndsWith(commandSuffix) =>
                        commandTypeName.Substring(0, commandTypeName.Length - commandSuffix.Length).ToLowerInvariant(),
                    _ => commandTypeName.ToLowerInvariant()
                };
                return true;
        }
    }
}




