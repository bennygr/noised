
namespace Noised.Plugins.FileSystemSource.DB
{
    static class MetaDataSql
    {
        internal static string CREATE_TABLE_STMT = @"CREATE TABLE IF NOT EXISTS [MetaData] (
													[ID] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
													[MediaItemId] INTEGER NOT NULL,
													[Title] NVARCHAR(1000),
													[Album] NVARCHAR(1000),
													[Comment] NVARCHAR(10000),
													[Year] INTEGER,
													[TrackNumber] INTEGER,
													[TrackCount] INTEGER,
													[Disc] INTEGER,
													[DiscCount] INTEGER,
													[Grouping] NVARCHAR(1000),
													[Lyrics] NVARCHAR(10000),
													[BeatsPerMinute] INTEGER,
													[Conductor] NVARCHAR(1000),
													[Copyright] NVARCHAR(1000)
													)";
        internal static string INSERT_STMT = @"INSERT INTO MetaData (MediaItemId,
											   Title,Album,Comment,Year,TrackNumber,TrackCount,
											   Disc,DiscCount,Grouping,Lyrics,BeatsPerMinute,Conductor,Copyright)
											   VALUES(@MediaItemId,
													 @Title,@Album, @Comment, @Year, @TrackNumber, @TrackCount,
													 @Disc,@DiscCount,@Grouping,@Lyrics,@BeatsPerMinute,@Conductor,@Copyright);";
		internal static string DELETE_STMT = @"DELETE FROM MetaData WHERE MediaItemId=@MediaItemId;";
        internal static string SELECT_STMT = @"SELECT ID, MediaItemId, Title, Album, Comment, Year, TrackNumber, TrackCount,
											Disc, DiscCount, Grouping, Lyrics, BeatsPerMinute, Conductor, Copyright 
											FROM MetaData WHERE MediaItemId=@MediaItemId;";


        internal static string CREATE_ARTISTS_TABLE_STMT = @"CREATE TABLE IF NOT EXISTS [MetaDataArtists] (
													[ID] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
													[MediaItemId] INTEGER NOT NULL,
													[Artist] NVARCHAR(1000)
													)";
        internal static string INSERT_ARTISTS_STMT = @"INSERT INTO MetaDataArtists (MediaItemId, Artist)
													VALUES(@MediaItemId, @Artist);";
		internal static string DELETE_ARTISTS_STMT = @"DELETE FROM MetaDataArtists WHERE MediaItemId=@MediaItemId;";
		internal static string SELECT_ARTISTS_STMT =  @"SELECT Artist FROM MetaDataArtists WHERE MediaItemId=@MediaItemId;";

        internal static string CREATE_ALBUM_ARTISTS_TABLE_STMT = @"CREATE TABLE IF NOT EXISTS [MetaDataAlbumArtists] (
													[ID] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
													[MediaItemId] INTEGER NOT NULL,
													[AlbumArtist] NVARCHAR(1000)
													)";
        internal static string INSERT_ALBUM_ARTISTS_STMT = @"INSERT INTO MetaDataAlbumArtists (MediaItemId, AlbumArtist)
													VALUES(@MediaItemId, @AlbumArtist);";
		internal static string DELETE_ALBUM_ARTISTS_STMT = @"DELETE FROM MetaDataAlbumArtists WHERE MediaItemId=@MediaItemId;";
		internal static string SELECT_ALBUM_ARTISTS_STMT = @"SELECT AlbumArtist FROM MetaDataAlbumArtists 
															WHERE MediaItemId=@MediaItemId;";

        internal static string CREATE_COMPOSER_TABLE_STMT = @"CREATE TABLE IF NOT EXISTS [MetaDataComposer] (
													[ID] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
													[MediaItemId] INTEGER NOT NULL,
													[Composer] NVARCHAR(1000)
													)";
        internal static string INSERT_COMPOSER_STMT = @"INSERT INTO MetaDataComposer (MediaItemId, Composer)
													VALUES(@MediaItemId, @Composer);";
		internal static string DELETE_COMPOSER_STMT = @"DELETE FROM MetaDataComposer WHERE MediaItemId=@MediaItemId;";
		internal static string SELECT_COMPOSER_STMT = @"SELECT Composer FROM MetaDataComposer WHERE MediaItemId=@MediaItemId;";

        internal static string CREATE_GENRE_TABLE_STMT = @"CREATE TABLE IF NOT EXISTS [MetaDataGenre] (
													[ID] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
													[MediaItemId] INTEGER NOT NULL,
													[Genre] NVARCHAR(1000)
													)";
        internal static string INSERT_GENRE_STMT = @"INSERT INTO MetaDataGenre (MediaItemId, Genre)
													VALUES(@MediaItemId, @Genre);";
		internal static string DELETE_GENRE_STMT = @"DELETE FROM MetaDataGenre WHERE MediaItemId=@MediaItemId;";
		internal static string SELECT_GENRE_STMT = @"SELECT Genre FROM MetaDataGenre WHERE MediaItemId=@MediaItemId;";
    };
}
