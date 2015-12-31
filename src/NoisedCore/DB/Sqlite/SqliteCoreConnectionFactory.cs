using Mono.Data.Sqlite;

namespace Noised.Core.DB.Sqlite
{
	class SqliteCoreConnectionFactory : ISqliteConnectionFactory
	{
		#region SqliteConnectionFactory
		
		public SqliteConnection Create()
		{
			var connectionString = "Data Source=" + SqliteFileSource.GetDBFileName();
			return new SqliteConnection(connectionString);
		}
		
		#endregion
	};
}
