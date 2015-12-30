namespace Noised.Core.DB.Sqlite
{
	static class MediaItemsSql
	{
		internal static string CREATE_TABLE_STMT = @"CREATE TABLE IF NOT EXISTS [MediaItems] (
											         [ID] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
													 [CHECKSUM] CHAR(32) NOT NULL,
													 [URI] NVARCHAR(500) NOT NULL UNIQUE)";
		internal static string INSERT_STMT = @"INSERT INTO MediaItems (URI,CHECKSUM) VALUES(@URI,@CHECKSUM)";
		internal static string GET_BY_URI_STMT = @"SELECT ID,URI FROM MediaItems WHERE URI=@URI";
	};
}
