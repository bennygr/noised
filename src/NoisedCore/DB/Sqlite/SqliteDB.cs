using System.Collections.Generic;

namespace Noised.Core.DB.Sqlite
{
    public class SqliteDB : IDB
    {
        private static List<string> GenerateCreateStatements()
        {
            return new List<string>
            {
                PluginsSql.CREATE_TABLE_STMT,
                PluginFilesSql.CREATE_TABLE_STMT,
                PlaylistSql.CreateTableStmt
            };
        }

        #region IDB implementation

        public void CreateOrUpdate()
        {
            new SqliteDBCreator().CreateOrUpdate(new SqliteCoreConnectionFactory(), GenerateCreateStatements());
        }

        #endregion
    };
}
