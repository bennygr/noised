using System;
using Mono.Data.Sqlite;

namespace Noised.Core.DB.Sqlite
{
    public static class SqliteUtils
    {
        public static Int64 GetLastInsertRowId(SqliteConnection connection, String tableName)
        {
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = "SELECT last_insert_rowid() FROM " + tableName;
                return (Int64)cmd.ExecuteScalar();
            }
        }
    };
}
