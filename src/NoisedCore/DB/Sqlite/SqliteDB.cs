using System;
using System.Collections.Generic;
using Mono.Data.Sqlite;
using Noised.Logging;
using Noised.Core.DB;
using Noised.Core.IOC;

namespace Noised.Core.DB.Sqlite
{
	public class SqliteDB : IDB
	{
	    private readonly ILogging logger;
	
		public SqliteDB()
		{
			logger = IocContainer.Get<ILogging>();
		}
	
	    private List<string> GenerateCreateStatements()
	    {
			return new List<string>
			{
				MetaDataSql.CREATE_ALBUM_ARTISTS_TABLE_STMT,
				MetaDataSql.CREATE_TABLE_STMT,
				MetaDataSql.CREATE_ARTISTS_TABLE_STMT,
				MetaDataSql.CREATE_COMPOSER_TABLE_STMT,
				MetaDataSql.CREATE_GENRE_TABLE_STMT,
				MediaItemsSql.CREATE_TABLE_STMT,
				PluginsSql.CREATE_TABLE_STMT,
				PluginFilesSql.CREATE_TABLE_STMT
			};	
	    }
	
	    #region IDB implementation
	
	    public void CreateOrUpdate()
	    {
	        using(var connection = SqliteConnectionFactory.Create())
			{
				connection.Open();
				foreach (var sql in GenerateCreateStatements())
	            {
					try
					{
						var command = new SqliteCommand(sql,connection);
						command.ExecuteNonQuery();
					}
					catch(Exception e)
					{
						logger.Error("Error while executing SQL: " + 
									 sql + 
									 " --> Error: " +
									 e.Message);
						throw;
					}
	            }	
			}
	    }
	
	    #endregion
	};
}
