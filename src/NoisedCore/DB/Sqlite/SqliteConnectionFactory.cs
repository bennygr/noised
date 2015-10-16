using System.Data.Common;
using Mono.Data.Sqlite;

namespace Noised.Core.DB.Sqlite
{
	static class SqliteConnectionFactory
	{
		internal static SqliteConnection Create()
		{
			var connectionString = "Data Source=" + SqliteFileSource.GetDBFileName();
			return new SqliteConnection(connectionString);
		}
	};
}
