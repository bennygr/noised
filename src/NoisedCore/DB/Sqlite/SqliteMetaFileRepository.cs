using System;
using System.Data;
using Mono.Data.Sqlite;
using Noised.Core.Media;

namespace Noised.Core.DB.Sqlite
{
    public class SqliteMetaFileRepository : IMetaFileRepository
    {
        private readonly SqliteConnection connection;

        public SqliteMetaFileRepository(SqliteConnection connection)
        {
            if (connection == null)
                throw new ArgumentNullException("connection");
            this.connection = connection;
        }

        #region Implementation of IMetaFileRepository

        public void CreateMetaFile(MetaFile metaFile)
        {
            if (metaFile == null)
                throw new ArgumentNullException("metaFile");

            using (SqliteCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = MetaFilesSql.InsertStmt;
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.Add(new SqliteParameter("@Artist", metaFile.Artist));
                cmd.Parameters.Add(new SqliteParameter("@Album", metaFile.Album));
                cmd.Parameters.Add(new SqliteParameter("@Type", metaFile.Type));
                cmd.Parameters.Add(new SqliteParameter("@Uri", metaFile.Uri));

                cmd.ExecuteNonQuery();
            }
        }

        #endregion
    }
}