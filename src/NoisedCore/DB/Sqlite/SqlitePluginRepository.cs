using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Mono.Data.Sqlite;
using Noised.Core.DB.Sqlite;
using Noised.Core.Plugins;

namespace Noised.Core.DB.Sqlite
{
	class SqlitePluginRepository : IPluginRepository
	{
	    private SqliteConnection connection;
	
	    internal SqlitePluginRepository(SqliteConnection connection)
	    {
	        this.connection = connection;
	    }
	
	    #region IPluginRegistrationRepository implementation
	
		public void RegisterPlugin(PluginMetaData pluginRegistration,List<FileInfo> files)	
	    {
			using(var cmd = connection.CreateCommand())
			{
				cmd.CommandText = PluginsSql.INSERT_REG_DATA_STMT;
				cmd.CommandType = CommandType.Text;
				cmd.Parameters.Add(new SqliteParameter("@Guid",pluginRegistration.Guid.ToString()));
				cmd.Parameters.Add(new SqliteParameter("@Version",pluginRegistration.Version));
				cmd.Parameters.Add(new SqliteParameter("@Name",pluginRegistration.Name));
				cmd.ExecuteNonQuery();
			}

			foreach(var file in files)
			{
				using(var cmd = connection.CreateCommand())
				{
					cmd.CommandText = PluginFilesSql.INSERT_FILE_STMT;
					cmd.CommandType = CommandType.Text;
					cmd.Parameters.Add(new SqliteParameter("@Guid",pluginRegistration.Guid.ToString()));
					cmd.Parameters.Add(new SqliteParameter("@File",file.FullName));
					cmd.ExecuteNonQuery();
				}
			}
	    }
	
	    public PluginMetaData GetByGuid(Guid guid)
	    {
			using(var cmd = connection.CreateCommand())
			{
				cmd.CommandText = PluginsSql.GET_BY_GUID_STMT;
				cmd.CommandType = CommandType.Text;
				String guidString = guid.ToString ("D");
				cmd.Parameters.Add(new SqliteParameter("@Guid",guidString));
				using(var reader = cmd.ExecuteReader())
				{
					if(reader.HasRows)
					{
						return new PluginMetaData
						{
							Name = (string)reader["Name"],
							Guid = Guid.Parse((string)reader["GUID"]),
							Version = (string)reader["Version"]
						};
					}
					return null;
				}
			}
	    }

		public List<FileInfo> GetRegisteredFilesForPlugin(Guid guid)
		{
			throw new NotImplementedException();
		}
	    #endregion
	};
}
