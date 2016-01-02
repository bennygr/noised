namespace Noised.Core.DB.Sqlite
{
	static class PluginsSql
	{
			internal static string CREATE_TABLE_STMT = @"CREATE TABLE IF NOT EXISTS [Plugins] (
														[GUID] CHAR(36) NOT NULL PRIMARY KEY,
														[Version] CHAR(7) NOT NULL,
														[Name] NVARCHAR(500) NOT NULL,
														[Description] NVARCHAR(1000),
														[Author] NVARCHAR(100),
														[AuthorEmail] NVARCHAR(100),
														[CreationDate] INTEGER);";

			internal static string GET_BY_GUID_STMT = @"SELECT GUID,Version,Name,
														Description,Author,AuthorEmail,CreationDate	
														FROM Plugins WHERE GUID=@Guid";

			internal static string INSERT_REG_DATA_STMT = @"INSERT INTO Plugins 
															(GUID,Version,Name,
															 Description,Author,AuthorEmail,CreationDate)
															VALUES
															(@Guid,@Version,@Name,
															 @Description,@Author,@AuthorEmail,@CreationDate);";

			internal static string DELETE_BY_GUID_STMT = @"DELETE FROM Plugins WHERE GUID=@Guid;";

	};
}
