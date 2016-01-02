using System.Collections.Generic;
using Noised.Core.DB;

namespace Noised.Core.DB.Sqlite
{
	public class SqliteDB : IDB
	{
	    private List<string> GenerateCreateStatements()
	    {
			return new List<string>
			{
				PluginsSql.CREATE_TABLE_STMT,
				PluginFilesSql.CREATE_TABLE_STMT
			};	
	    }
	
	    #region IDB implementation
	
	    public void CreateOrUpdate()
	    {
			new SqliteDBCreator().CreateOrUpdate(new SqliteCoreConnectionFactory(),GenerateCreateStatements());
	    }
	
	    #endregion
	};
}
