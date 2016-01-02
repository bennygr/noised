namespace Noised.Plugins.FileSystemSource.DB
{
    static class MediaItemsSql
    {
        internal static string CREATE_TABLE_STMT = @"CREATE TABLE IF NOT EXISTS [MediaItems] (
											         [ID] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
													 [CHECKSUM] CHAR(32) NOT NULL,
													 [URI] NVARCHAR(500) NOT NULL UNIQUE)";
        internal static string INSERT_STMT = @"INSERT INTO MediaItems (URI,CHECKSUM) VALUES(@URI,@CHECKSUM)";
        internal static string DELETE_STMT = @"DELETE FROM MediaItems WHERE ID=@MediaItemId;";
        internal static string GET_BY_URI_STMT = @"SELECT ID,CHECKSUM,URI FROM MediaItems WHERE URI=@URI";
        internal static string FIND_BY_TITLE_STMT = @"SELECT item.ID,item.CHECKSUM,item.URI FROM mediaitems item, metadata meta 
													 WHERE item.id=meta.MediaItemId AND
													 meta.Title LIKE @PATTERN;";
    };
}
