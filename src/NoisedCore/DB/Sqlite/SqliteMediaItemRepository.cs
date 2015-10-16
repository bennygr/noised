using System;
using System.Data;
using Mono.Data.Sqlite;
using Noised.Core.DB;
using Noised.Core.Media;

namespace Noised.Core.DB.Sqlite
{
    class SqliteMediaItemRepository : IMediaItemRepository
    {
		private SqliteConnection connection;

		/// <summary>
		///		Constructor
		/// </summary>
		/// <param name="connection">The connection to use</param>
		internal SqliteMediaItemRepository(SqliteConnection connection)
		{
			this.connection = connection; 
		}

        #region IMediaItemRepository implementation

        public void Create(MediaItem item)
        {
			//Creating Media items
            using(var cmd = connection.CreateCommand())
			{
				 cmd.CommandText = MediaItemsSql.INSERT_STMT;
				 cmd.CommandType = CommandType.Text;
				 cmd.Parameters.Add(new SqliteParameter("@URI",item.Uri.ToString()));
				 cmd.ExecuteNonQuery();
				 item.Id = SqliteUtils.GetLastInsertRowId(connection,"MediaItems");
			}

			//And the media items' meta data
			foreach(var metaData in item.MetaData)
			{
				using(var cmd =connection.CreateCommand())
				{
					cmd.CommandText = MetaDataSql.INSERT_STMT;
					cmd.CommandType = CommandType.Text;
					cmd.Parameters.Add(new SqliteParameter("@MediaItemId",item.Id));
					cmd.Parameters.Add(new SqliteParameter("@Name",metaData.Name));
					cmd.Parameters.Add(new SqliteParameter("@Value",metaData.Value));
					cmd.ExecuteNonQuery();
				}
			}
        }

        public MediaItem GetByUri(Uri uri)
        {
            using(var cmd = connection.CreateCommand())
			{
				cmd.CommandText = MediaItemsSql.GET_BY_URI_STMT;
				cmd.Parameters.Add(new SqliteParameter("@URI",uri.ToString()));
				using(var reader = cmd.ExecuteReader())
				{
					if(reader.HasRows)
					{
						reader.Read();
						var id = (Int64)reader["ID"];
						var testUri = (string)reader["URI"];
						Console.WriteLine(" III-> " + id);
						Console.WriteLine(" III-> " + testUri);
						//TODO create item and return IT!
					}
				}
			}

			return null;
        }

        #endregion
    };
}
