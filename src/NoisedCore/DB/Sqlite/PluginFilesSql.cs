
namespace Noised.Core.DB.Sqlite
{
	static class PluginFilesSql
	{
			internal static string CREATE_TABLE_STMT = @"CREATE TABLE IF NOT EXISTS [PluginFiles] (
														[GUID] CHAR(36) NOT NULL PRIMARY KEY,
														[File] NVARCHAR(500) NOT NULL)";
	};
}
