namespace Noised.Core.DB.Sqlite
{
    internal static class MetaFilesSql
    {
        internal const string CreateTableStmt = "CREATE TABLE IF NOT EXISTS [MetaFiles] (" +
                                                "[ID] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT," +
                                                "[Artist] TEXT NOT NULL," +
                                                "[Album] TEXT," +
                                                "[Type] TEXT NOT NULL," +
                                                "[Uri] TEXT NOT NULL);";

        internal const string InsertStmt = "INSERT INTO MetaFiles (Artist, Album, Type, Uri) VALUES (@Artist, @Album, @Type, @Uri)";
    }
}
