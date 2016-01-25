using System;
using System.Collections.Generic;
using System.Data;
using Mono.Data.Sqlite;
using Noised.Core.DB.Sqlite;
using Noised.Core.Media;

namespace Noised.Plugins.FileSystemSource.DB
{
    class SqliteMediaItemRepository : IMediaItemRepository
    {
        private readonly SqliteConnection connection;

        /// <summary>
        ///		Constructor
        /// </summary>
        /// <param name="connection">The connection to use</param>
        internal SqliteMediaItemRepository(SqliteConnection connection)
        {
            this.connection = connection;
        }

        /// <summary>
        ///		Internal method for deleting data related to a MediaItem	
        /// </summary>
        private void deleteData(string statement, MediaItem mediaItem)
        {
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = statement;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new SqliteParameter("@MediaItemId", mediaItem.Id));
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        ///		Internal method to read a raw MediaItem 
        /// </summary>
        /// <returns>The count of items found</returns>
        private int GetMediaItems(string statement, IList<MediaItem> ret, IEnumerable<SqliteParameter> parameters)
        {
            int count = 0;
            //MediaItem
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = statement;
                foreach (var parameter in parameters)
                {
                    cmd.Parameters.Add(parameter);
                }
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var id = (Int64)reader["ID"];
                        var checksum = (string)reader["CHECKSUM"];
                        var itemUri = (string)reader["URI"];
                        var mediaItem = new MediaItem(new Uri(itemUri), checksum);
                        mediaItem.Id = id;
                        //Reading metadata
                        mediaItem.MetaData = GetMetaData(mediaItem);
                        ret.Add(mediaItem);
                        count++;
                    }
                }
            }
            return count;
        }

        /// <summary>
        ///		Internal method for reading meta data for a given MediaItem
        /// </summary>
        private MetaData GetMetaData(MediaItem mediaItem)
        {
            MetaData metaData = null;

            //Basic meta data
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = MetaDataSql.SELECT_STMT;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new SqliteParameter("@MediaItemId", mediaItem.Id));
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        metaData = new MetaData();
                        reader.Read();
                        metaData.Album = reader["Album"] == DBNull.Value ? null : (string)reader["Album"];
                        metaData.BeatsPerMinute =
                                reader["BeatsPerMinute"] == DBNull.Value ? 0 : Convert.ToUInt32(reader["BeatsPerMinute"]);
                        metaData.Comment = reader["Comment"] == DBNull.Value ? null : (string)reader["Comment"];
                        metaData.Conductor = reader["Conductor"] == DBNull.Value ? null : (string)reader["Conductor"];
                        metaData.Copyright = reader["Copyright"] == DBNull.Value ? null : (string)reader["Copyright"];
                        metaData.Disc =
                                reader["Disc"] == DBNull.Value ? 0 : Convert.ToUInt32(reader["Disc"]);
                        metaData.DiscCount =
                                reader["DiscCount"] == DBNull.Value ? 0 : Convert.ToUInt32(reader["DiscCount"]);
                        metaData.Grouping = reader["Grouping"] == DBNull.Value ? null : (string)reader["Grouping"];
                        metaData.Lyrics = reader["Lyrics"] == DBNull.Value ? null : (string)reader["Lyrics"];
                        metaData.Title = reader["Title"] == DBNull.Value ? null : (string)reader["Title"];
                        metaData.TrackCount =
                                reader["TrackCount"] == DBNull.Value ? 0 : Convert.ToUInt32(reader["TrackCount"]);
                        metaData.TrackNumber =
                                reader["TrackNumber"] == DBNull.Value ? 0 : Convert.ToUInt32(reader["TrackNumber"]);
                        metaData.Year =
                                reader["Year"] == DBNull.Value ? 0 : Convert.ToUInt32(reader["Year"]);
                    }
                }
            }

            if (metaData != null)
            {
                //Further meta data from other tables
                metaData.AlbumArtists = readStringList(MetaDataSql.SELECT_ALBUM_ARTISTS_STMT, "AlbumArtist", mediaItem);
                metaData.Artists = readStringList(MetaDataSql.SELECT_ARTISTS_STMT, "Artist", mediaItem);
                metaData.Composers = readStringList(MetaDataSql.SELECT_COMPOSER_STMT, "Composer", mediaItem);
                metaData.Genres = readStringList(MetaDataSql.SELECT_GENRE_STMT, "Genre", mediaItem);
            }
            return metaData;
        }


        /// <summary>
        ///		Internal Method for reading a list of meta data for a given MediaItem
        /// </summary>
        private List<string> readStringList(string statement, string columnName, MediaItem mediaItem)
        {
            var ret = new List<string>();
            //List of ALbum-Artists
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = statement;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new SqliteParameter("@MediaItemId", mediaItem.Id));
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ret.Add((string)reader[columnName]);
                    }
                }
            }
            return ret;
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

        public void Delete(MediaItem item)
        {
            deleteData(MetaDataSql.DELETE_STMT, item);
            deleteData(MetaDataSql.DELETE_GENRE_STMT, item);
            deleteData(MetaDataSql.DELETE_ARTISTS_STMT, item);
            deleteData(MetaDataSql.DELETE_COMPOSER_STMT, item);
            deleteData(MetaDataSql.DELETE_ALBUM_ARTISTS_STMT, item);
            deleteData(MediaItemsSql.DELETE_STMT, item);
        }

        public MediaItem GetByUri(Uri uri)
        {
            var mediaItems = new List<MediaItem>();
            GetMediaItems(MediaItemsSql.GET_BY_URI_STMT,
                mediaItems,
                new List<SqliteParameter>
                {
                    new SqliteParameter("@URI", uri.ToString()) 
                });

            return mediaItems.Count > 0 ? mediaItems[0] : null;
        }

        public int FindByTitle(string title, IList<MediaItem> ret)
        {
            return GetMediaItems(MediaItemsSql.FIND_BY_TITLE_STMT,
                ret,
                new List<SqliteParameter>
                {
                    new SqliteParameter("@PATTERN", "%" + title + "%")
                });
        }

        public int FindByArtist(string artist, IList<MediaItem> ret)
        {
            return GetMediaItems(MediaItemsSql.FIND_BY_ARTIST_STMT,
                ret,
                new List<SqliteParameter>
                {
                    new SqliteParameter("@PATTERN", "%" + artist + "%")
                });
        }

        public int FindByAlbum(string album, IList<MediaItem> ret)
        {
            return GetMediaItems(MediaItemsSql.FIND_BY_ALBUM_STMT,
                ret,
                new List<SqliteParameter>
                {
                    new SqliteParameter("@PATTERN", "%" + album + "%")
                });
        }


        #endregion
    };
}
