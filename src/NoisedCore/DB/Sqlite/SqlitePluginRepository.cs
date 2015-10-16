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
	
		public void RegisterPlugin(PluginRegistrationData pluginRegistration,List<FileInfo> files)	
	    {
	        throw new System.NotImplementedException();
	    }
	
	    public PluginRegistrationData GetByGuid(Guid guid)
	    {
			using(var cmd = connection.CreateCommand())
			{
				cmd.CommandText = PluginRegistrationSql.GET_BY_GUID_STMT;
				cmd.CommandType = CommandType.Text;
				cmd.Parameters.Add(new SqliteParameter("@Guid",guid.ToString()));
				using(var reader = cmd.ExecuteReader())
				{
					if(reader.HasRows)
					{
						return new PluginRegistrationData
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
