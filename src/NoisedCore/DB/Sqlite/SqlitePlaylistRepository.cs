using System;
using System.Collections.Generic;
using System.Data;
using Mono.Data.Sqlite;
using Noised.Core.IOC;
using Noised.Core.Media;
using System.Linq;

namespace Noised.Core.DB.Sqlite
{
    /// <summary>
    /// IPlaylistRepository implementation for Sqlite
    /// </summary>
    class SqlitePlaylistRepository : IPlaylistRepository
    {
        private readonly SqliteConnection connection;

        /// <summary>
        /// IPlaylistRepository implementation for Sqlite
        /// </summary>
        /// <param name="connection">Connection to a Sqlite Database</param>
        public SqlitePlaylistRepository(SqliteConnection connection)
        {
            if (connection == null)
                throw new ArgumentNullException("connection");

            this.connection = connection;
        }

        private IEnumerable<Listable<MediaItem>> GetItemsForList(long listId)
        {
            var factory = IoC.Get<IMediaSourceAccumulator>();
            using (SqliteCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = PlaylistsSql.SelectPlaylistItems;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new SqliteParameter("@ID", listId));
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var uri = new Uri((string)reader["MediaItemUri"]);
                    yield return new Listable<MediaItem>(factory.Get(uri));
                }
            }
        }

        #region Implementation of IPlaylistRepository

        public void Create(Playlist playlist)
        {
            if (playlist == null)
                throw new ArgumentNullException("playlist");

            //Adding the playlist to the DB
            using (SqliteCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = PlaylistsSql.InsertPlaylistStatement;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new SqliteParameter("@Name", playlist.Name));
                cmd.ExecuteNonQuery();
            }
            playlist.Id = SqliteUtils.GetLastInsertRowId(connection, "Playlists");

            //Writing the playlist's items
            foreach (Listable<MediaItem> mediaItem in playlist.Items)
            {
                using (SqliteCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = PlaylistsSql.InsertPlaylistItemStatement;
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.Add(new SqliteParameter("@ID", playlist.Id));
                    cmd.Parameters.Add(new SqliteParameter("@MediaItemUri", mediaItem.Item.Uri));

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Update(Playlist playlist)
        {
            if (playlist == null)
                throw new ArgumentNullException("playlist");

            Delete(playlist);
            Create(playlist);
        }

        public void Delete(Playlist playlist)
        {
            if (playlist == null)
                throw new ArgumentNullException("playlist");

            //Removing the playlist's items
            using (SqliteCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = PlaylistsSql.DeletePlaylistItemsStatement;
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.Add(new SqliteParameter("@ID", playlist.Id));
                cmd.ExecuteNonQuery();
            }

            //removing the playlist
            using (SqliteCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = PlaylistsSql.DeletePlaylistStatement;
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.Add(new SqliteParameter("@ID", playlist.Id));
                cmd.ExecuteNonQuery();
            }
        }

        public IList<Playlist> GetAll()
        {
            var ret = new List<Playlist>();
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = PlaylistsSql.SelectAllPlaylists;
                cmd.CommandType = CommandType.Text;
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var name = (string)reader["Name"];
                    var id = (Int64)reader["ID"];
                    var playlist = new Playlist(name){ Id = id };
                    ret.Add(playlist);
                }
            }
            foreach(Playlist playlist in ret)
            {
                playlist.Add(GetItemsForList(playlist.Id).ToArray());
            }
            return ret;
        }

        public Playlist GetById(Int64 id)
        {
            Playlist playlist = null;
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = PlaylistsSql.SelectPlaylist;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new SqliteParameter("@ID", id));
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    var name = (string)reader["Name"];
                    playlist = new Playlist(name){ Id = id };
                }
            }
            if(playlist != null)
            {
                playlist.Add(GetItemsForList(playlist.Id).ToArray());
            }
            return playlist;
        }

        #endregion
    }
}
