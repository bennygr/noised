namespace Noised.Core.DB.Sqlite
{
    internal static class MetaFilesSql
    {
        internal const string CreateTableStmt = "CREATE TABLE IF NOT EXISTS [MetaFiles] (" +
                                                "[ID] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT," +
                                                "[Artist] TEXT NOT NULL," +
                                                "[Album] TEXT," +
                                                "[Type] TEXT NOT NULL," +
                                                "[Uri] TEXT NOT NULL," +
                                                "[Category] TEXT NOT NULL);";

        internal const string InsertStmt = "INSERT INTO MetaFiles (Artist, Album, Type, Uri, Category) VALUES (@Artist, @Album, @Type, @Uri, @Category)";

        internal const string SelectStmt = "SELECT Artist, Album, Type, Uri, Category FROM MetaFiles";

        internal const string SelectWhereStmt = "SELECT Artist, Album, Type, Uri, Category FROM MetaFiles where Artist = @Artist and Album = @Album";

        internal const string DeleteStmnt = "DELETE FROM MetaFiles where Uri = @Uri";
    }
}
