namespace Noised.Core.DB.Sqlite
{
	static class PluginRegistrationSql
	{
			internal static string CREATE_TABLE_STMT = @"CREATE TABLE IF NOT EXISTS [PluginRegistrations] (
														[GUID] CHAR(36) NOT NULL PRIMARY KEY,
														[Version] CHAR(7) NOT NULL,
														[Name] NVARCHAR(500) NOT NULL)";
			internal static string GET_BY_GUID_STMT = @"SELECT GUID,Version,Name FROM PluginRegistrations WHERE GUID=@Guid";

			internal static string INSERT_REG_DATA_STMT = @"INSERT INTO PluginRegistrations 
															(GUID,Version,Name)
															VALUES
															(@Guid,@Version,@Name);";

	};
}
