using System;
using Mono.Data.Sqlite;
using Noised.Core.Media;

namespace Noised.Core.DB.Sqlite
{
    internal class SqlitePlaylistRepository : IPlaylistRepository
    {
        private readonly SqliteConnection connection;

        public SqlitePlaylistRepository(SqliteConnection connection)
        {
            if (connection == null)
                throw new ArgumentNullException("connection");
            this.connection = connection;
        }

        #region Implementation of IPlaylistRepository

        public void CreatePlaylist(Playlist playlist)
        {
            if (playlist == null)
                throw new ArgumentNullException("playlist");
        }

        public Playlist ReadPlaylist(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
                throw new ArgumentException(strings.NoValidPlaylistName);

            return null;
        }

        public void UpdatePlaylist(Playlist playlist)
        {
            if (playlist == null)
                throw new ArgumentNullException("playlist");
        }

        public void DeletePlaylist(Playlist playlist)
        {
            if (playlist == null)
                throw new ArgumentNullException("playlist");
        }

        #endregion
    }
}
