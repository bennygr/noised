using Mono.Data.Sqlite;
using Noised.Core.DB.Sqlite;

/// <summary>
///		Factory for creating sqlite DB storing Filesystem items
/// </summary>
class SqliteFileSystemSourceConnectionFactory : ISqliteConnectionFactory
{
    private const string NOISED_FILESYSTEM_SOURCE_DB_FILE_NAME = "noisedfs.db";

    #region ISqliteConnectionFactory implementation

    public SqliteConnection Create()
    {
        var connectionString = "Data Source=" + NOISED_FILESYSTEM_SOURCE_DB_FILE_NAME;
        return new SqliteConnection(connectionString);
    }

    #endregion
};
