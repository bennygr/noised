using System;
using System.Collections.Generic;
using System.Data;
using Mono.Data.Sqlite;
using Noised.Core.Commands;
using Noised.Core.IOC;
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

            foreach (MediaItem mediaItem in playlist.Items)
            {
                using (SqliteCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = PlaylistSql.InsertPlaylistStatement;
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.Add(new SqliteParameter("@Name", playlist.Name));
                    cmd.Parameters.Add(new SqliteParameter("@MediaItemUri", mediaItem.Uri));

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdatePlaylist(Playlist playlist)
        {
            if (playlist == null)
                throw new ArgumentNullException("playlist");

            DeletePlaylist(playlist);
            CreatePlaylist(playlist);
        }

        public void DeletePlaylist(Playlist playlist)
        {
            if (playlist == null)
                throw new ArgumentNullException("playlist");

            using (SqliteCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = PlaylistSql.DeletePlaylistStatement;
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.Add(new SqliteParameter("@Name", playlist.Name));

                cmd.ExecuteNonQuery();
            }
        }

        public List<Playlist> GetAllPlaylists()
        {
            DataTable playlistTable = new DataTable();

            using (SqliteCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = PlaylistSql.SelectAllPlaylists;
                cmd.CommandType = CommandType.Text;

                playlistTable.Load(cmd.ExecuteReader());
            }

            List<Playlist> playlists = new List<Playlist>();
            Playlist p;
            IMediaSourceAccumulator mediaSourceAccumulator = IocContainer.Get<IMediaSourceAccumulator>();
            foreach (DataRow row in playlistTable.Rows)
            {
                p = playlists.Find(x => x.Name == row["Name"].ToString());

                if (p == null)
                {
                    p = new Playlist(row["Name"].ToString());
                    playlists.Add(p);
                }

                p.Add(mediaSourceAccumulator.Get(new Uri(row["MediaItemUri"].ToString())));
            }

            return playlists;
        }

        #endregion
    }
}
