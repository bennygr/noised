using System.Collections.Generic;
using Noised.Core.DB.Sqlite;
using Noised.Plugins.Media.FileSystemSource.DB.Sqlite;

namespace Noised.Plugins.FileSystemSource.DB
{
    /// <summary>
    ///		Class for creatring the Filesystem Source Plugin's database
    /// </summary>
    class FileSystemDBCreator
    {
        private List<string> GenerateCreateStatements()
        {
            return new List<string>
	        {
				MediaItemsSql.CREATE_TABLE_STMT,
				MetaDataSql.CREATE_TABLE_STMT,
				MetaDataSql.CREATE_ARTISTS_TABLE_STMT,
				MetaDataSql.CREATE_ALBUM_ARTISTS_TABLE_STMT,
				MetaDataSql.CREATE_COMPOSER_TABLE_STMT,
				MetaDataSql.CREATE_GENRE_TABLE_STMT
	        };
        }

        internal void CreateOrUpdate()
        {
            new SqliteDBCreator().CreateOrUpdate(new SqliteFileSystemSourceConnectionFactory(), GenerateCreateStatements());
        }
    };
}
