
namespace Noised.Core.DB.Sqlite
{
	static class MetaDataSql
	{
		internal static string CREATE_TABLE_STMT = @"CREATE TABLE IF NOT EXISTS [MetaData] (
													[ID] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
													[MediaItemId] INTEGER NOT NULL,
													[Name] NVARCHAR(500) NOT NULL,
													[Value] VARCHAR(2048)  NULL)";
		internal static string INSERT_STMT = @"INSERT INTO MetaData (MediaItemId,Name,Value) 
											   VALUES(@MediaItemId,@Name,@Value)";
	};
}
