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

        public void RegisterPlugin(PluginMetaData pluginRegistration, List<FileInfo> files)
        {
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = PluginsSql.INSERT_REG_DATA_STMT;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new SqliteParameter("@Guid", pluginRegistration.Guid));
                cmd.Parameters.Add(new SqliteParameter("@Version", pluginRegistration.Version));
                cmd.Parameters.Add(new SqliteParameter("@Name", pluginRegistration.Name));
                cmd.Parameters.Add(new SqliteParameter("@Description", pluginRegistration.Description));
                cmd.Parameters.Add(new SqliteParameter("@Author", pluginRegistration.Author));
                cmd.Parameters.Add(new SqliteParameter("@AuthorEmail", pluginRegistration.AuthorEmail));
				if(pluginRegistration.CreationDate.HasValue)
					cmd.Parameters.Add(new SqliteParameter("@CreationDate", pluginRegistration.CreationDate.Value.Ticks));
				else
					cmd.Parameters.Add(new SqliteParameter("@CreationDate", null));
                cmd.ExecuteNonQuery();
            }

            foreach (var file in files)
            {
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = PluginFilesSql.INSERT_FILE_STMT;
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add(new SqliteParameter("@Guid", pluginRegistration.Guid.ToString()));
                    cmd.Parameters.Add(new SqliteParameter("@File", file.FullName));
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UnregisterPlugin(PluginMetaData pluginRegistration)
        {
            //Files
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = PluginFilesSql.DELETE_FILE_BY_GUID_STMT;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new SqliteParameter("@Guid", pluginRegistration.Guid.ToString()));
                cmd.ExecuteNonQuery();
            }

            //Plugin
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = PluginsSql.DELETE_BY_GUID_STMT;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new SqliteParameter("@Guid", pluginRegistration.Guid.ToString()));
                cmd.ExecuteNonQuery();
            }
        }
	
        public PluginMetaData GetByGuid(Guid guid)
        {
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = PluginsSql.GET_BY_GUID_STMT;
                cmd.CommandType = CommandType.Text;
                String guidString = guid.ToString("D");
                cmd.Parameters.Add(new SqliteParameter("@Guid", guidString));
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        return new PluginMetaData
                        {
                            Name = (string)reader["Name"],
                            Guid = (string)reader["GUID"],
                            Version = (string)reader["Version"],
							Description = reader["Description"] == DBNull.Value ? null: (string)reader["Description"],
							Author = reader["Author"] == DBNull.Value ? null : (string)reader["Author"],
							AuthorEmail = reader["AuthorEmail"] == DBNull.Value ? null :(string)reader["AuthorEmail"],
							CreationDate = reader["CreationDate"] == DBNull.Value ? (DateTime?)null : new DateTime((long)reader["CreationDate"])
                        };
                    }
                    return null;
                }
            }
        }

        public PluginMetaData GetForFile(FileInfo file)
        {
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = PluginFilesSql.GET_GUID_FOR_FILE;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new SqliteParameter("@File", file.FullName));
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
						var guidString = (string)reader["GUID"];
						return GetByGuid(Guid.Parse(guidString));
                    }
                }
            }
			return null;
        }

        public List<FileInfo> GetRegisteredFilesForPlugin(Guid guid)
        {
            throw new NotImplementedException();
        }

        #endregion
    };
}
