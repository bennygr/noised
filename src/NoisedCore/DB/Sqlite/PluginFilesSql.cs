namespace Noised.Core.DB.Sqlite
{
	static class PluginFilesSql
	{
			internal static string CREATE_TABLE_STMT = @"CREATE TABLE IF NOT EXISTS [PluginFiles] (
														[GUID] CHAR(36) NOT NULL,
														[File] NVARCHAR(500) NOT NULL)";
			internal static string INSERT_FILE_STMT = @"INSERT INTO PluginFiles (GUID,File) VALUES (@Guid,@File);";
			internal static string GET_GUID_FOR_FILE_STMT = @"SELECT GUID FROM PluginFiles WHERE File=@FILE;";
			internal static string GET_ALL_FILES_STMT = @"SELECT GUID,FILE FROM PluginFiles";
			internal static string DELETE_FILE_BY_GUID_STMT = @"DELETE FROM PluginFiles WHERE GUID=@Guid;";
	};
}
