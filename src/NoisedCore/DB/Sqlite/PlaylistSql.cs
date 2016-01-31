namespace Noised.Core.DB.Sqlite
{
    internal static class PlaylistSql
    {
        internal const string CreateTableStmt = "CREATE TABLE IF NOT EXISTS [Playlists] (" +
                                                "[Name] TEXT NOT NULL," +
                                                "[MediaItemUri] TEXT NOT NULL," +
                                                "PRIMARY KEY (Name, MediaItemUri));";

        internal const string InsertPlaylistStatement =
            "INSERT INTO Playlists (Name, MediaItemUri) VALUES (@Name, @MediaItemUri);";

        internal const string DeletePlaylistStatement = "DELETE FROM Playlists WHERE Name = @Name;";

        internal const string SelectAllPlaylists = "SELECT Name, MediaItemUri FROM Playlists;";
    }
}
