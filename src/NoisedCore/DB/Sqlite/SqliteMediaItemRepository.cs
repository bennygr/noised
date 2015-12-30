using System;
using System.Data;
using Mono.Data.Sqlite;
using Noised.Core.DB;
using Noised.Core.Media;

namespace Noised.Core.DB.Sqlite
{
    class SqliteMediaItemRepository : IMediaItemRepository
    {
        private SqliteConnection connection;

        /// <summary>
        ///		Constructor
        /// </summary>
        /// <param name="connection">The connection to use</param>
        internal SqliteMediaItemRepository(SqliteConnection connection)
        {
            this.connection = connection; 
        }

        #region IMediaItemRepository implementation

        public void Create(MediaItem item)
        {
            //Creating Media items
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = MediaItemsSql.INSERT_STMT;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new SqliteParameter("@URI", item.Uri.ToString()));
                cmd.Parameters.Add(new SqliteParameter("@CHECKSUM", item.Checksum));
                cmd.ExecuteNonQuery();
                item.Id = SqliteUtils.GetLastInsertRowId(connection, "MediaItems");
            }

            //
            if (item.MetaData != null)
            {
                var metaData = item.MetaData;

                //Main Meta Data
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = MetaDataSql.INSERT_STMT;
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add(new SqliteParameter("@MediaItemId", item.Id));
                    cmd.Parameters.Add(new SqliteParameter("@Title", metaData.Title));
                    cmd.Parameters.Add(new SqliteParameter("@Album", metaData.Album));
                    cmd.Parameters.Add(new SqliteParameter("@Comment", metaData.Comment));
                    cmd.Parameters.Add(new SqliteParameter("@Year", metaData.Year));
                    cmd.Parameters.Add(new SqliteParameter("@TrackNumber", metaData.TrackNumber));
                    cmd.Parameters.Add(new SqliteParameter("@TrackCount", metaData.TrackCount));
                    cmd.Parameters.Add(new SqliteParameter("@Disc", metaData.Disc));
                    cmd.Parameters.Add(new SqliteParameter("@DiscCount", metaData.DiscCount));
                    cmd.Parameters.Add(new SqliteParameter("@Grouping", metaData.Grouping));
                    cmd.Parameters.Add(new SqliteParameter("@Lyrics", metaData.Lyrics));
                    cmd.Parameters.Add(new SqliteParameter("@BeatsPerMinute", metaData.BeatsPerMinute));
                    cmd.Parameters.Add(new SqliteParameter("@Conductor", metaData.Conductor));
                    cmd.Parameters.Add(new SqliteParameter("@Copyright", metaData.Copyright));
                    cmd.ExecuteNonQuery();
                }

                //Artist meta data
                foreach (var artist in metaData.Artists)
                {
                    using (var cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = MetaDataSql.INSERT_ARTISTS_STMT;
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add(new SqliteParameter("@MediaItemId", item.Id));
                        cmd.Parameters.Add(new SqliteParameter("@Artist", artist));
                        cmd.ExecuteNonQuery();
                    }
                }

				//Album artists
                foreach (var albumArtist in metaData.AlbumArtists)
                {
                    using (var cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = MetaDataSql.INSERT_ALBUM_ARTISTS_STMT;
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add(new SqliteParameter("@MediaItemId", item.Id));
                        cmd.Parameters.Add(new SqliteParameter("@AlbumArtist", albumArtist));
                        cmd.ExecuteNonQuery();
                    }
                }

				//Composers
                foreach (var composer in metaData.Composers)
                {
                    using (var cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = MetaDataSql.INSERT_COMPOSER_STMT;
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add(new SqliteParameter("@MediaItemId", item.Id));
                        cmd.Parameters.Add(new SqliteParameter("@Composer", composer));
                        cmd.ExecuteNonQuery();
                    }
                }

				//Genres
                foreach (var genre in metaData.Genres)
                {
                    using (var cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = MetaDataSql.INSERT_GENRE_STMT;
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add(new SqliteParameter("@MediaItemId", item.Id));
                        cmd.Parameters.Add(new SqliteParameter("@Genre", genre));
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public MediaItem GetByUri(Uri uri)
        {
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = MediaItemsSql.GET_BY_URI_STMT;
                cmd.Parameters.Add(new SqliteParameter("@URI", uri.ToString()));
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        var id = (Int64)reader["ID"];
                        var testUri = (string)reader["URI"];
                        Console.WriteLine(" III-> " + id);
                        Console.WriteLine(" III-> " + testUri);
                        //TODO create item and return IT!
                    }
                }
            }

            return null;
        }

        #endregion
    };
}
