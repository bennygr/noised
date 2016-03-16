using System;
using System.Data;
using Mono.Data.Sqlite;
using Noised.Core.Media;

namespace Noised.Core.DB.Sqlite
{
    /// <summary>
    /// A MetaFileRepository for SQLite
    /// </summary>
    public class SqliteMetaFileRepository : IMetaFileRepository
    {
        private readonly SqliteConnection connection;

        /// <summary>
        /// A MetaFileRepository for SQLite
        /// </summary>
        /// <param name="connection">The SqliteConnection</param>
        public SqliteMetaFileRepository(SqliteConnection connection)
        {
            if (connection == null)
                throw new ArgumentNullException("connection");
            this.connection = connection;
        }

        #region Implementation of IMetaFileRepository

        /// <summary>
        /// Creates a new entry for a MetaFile in the current IUnitOfWork implementation
        /// </summary>
        /// <param name="metaFile">MetaFile for which an entry should be created</param>
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
                cmd.Parameters.Add(new SqliteParameter("@Type", metaFile.Type.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@Uri", metaFile.Uri));
                cmd.Parameters.Add(new SqliteParameter("@Category", metaFile.Category.ToString()));

                cmd.ExecuteNonQuery();
            }
        }

        #endregion
    }
}