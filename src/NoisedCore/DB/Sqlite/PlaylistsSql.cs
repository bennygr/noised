namespace Noised.Core.DB.Sqlite
{
    internal static class PlaylistsSql
    {
        internal const string CreateTableStmt = "CREATE TABLE IF NOT EXISTS [Playlists] (" +
                                                "[ID] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT," +
                                                "[Name] TEXT NOT NULL);";

        internal const string CreateItemsTableStmt = "CREATE TABLE IF NOT EXISTS [PlaylistItems] (" +
                                                "[ID] TEXT NOT NULL," +
                                                "[MediaItemUri] TEXT NOT NULL, " + 
                                                "PRIMARY KEY (ID, MediaItemUri))";

        internal const string InsertPlaylistStatement = "INSERT INTO Playlists (Name) VALUES (@Name);";
        internal const string InsertPlaylistItemStatement = "INSERT INTO PlaylistItems (ID,MediaItemUri) VALUES (@ID,@MediaItemUri);";

        internal const string DeletePlaylistStatement = "DELETE FROM Playlists WHERE ID = @ID;";
        internal const string DeletePlaylistItemsStatement = "DELETE FROM PlaylistItems WHERE ID=@ID;";

        //internal const string SelectAllPlaylists = "SELECT Name, MediaItemUri FROM Playlists;";
        
        internal const string SelectAllPlaylists = "SELECT ID,Name from Playlists;";
        internal const string SelectPlaylistItems = "SELECT ID,MediaItemUri from PlaylistItems WHERE ID=@ID;";

        internal const string SelectPlaylist = "SELECT ID,Name from Playlists WHERE ID=@ID";
    }
}
