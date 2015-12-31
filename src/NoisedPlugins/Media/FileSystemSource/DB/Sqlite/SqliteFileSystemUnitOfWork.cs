using Mono.Data.Sqlite;
using Noised.Plugins.FileSystemSource.DB;

namespace Noised.Plugins.FileSystemSource.DB.Sqlite
{
    /// <summary>
    ///		Sqlite implementation of  IFileSystemUnitOfWork
    /// </summary>
    class SqliteFileSystemUnitOfWork : IFileSystemUnitOfWork
    {
        private readonly SqliteConnection connection;
        private IMediaItemRepository mediaItemRepository;
        private SqliteTransaction transaction;

        internal SqliteFileSystemUnitOfWork()
        {
            connection = new SqliteFileSystemSourceConnectionFactory().Create();		
            connection.Open();
            transaction = connection.BeginTransaction();
        }

        #region IDisposable implementation

        public void Dispose()
        {
            if (connection != null)
            {
                connection.Close();
            }
        }

        #endregion

        #region IFileSystemUnitOfWork implementation

        public void SaveChanges()
        {
            transaction.Commit();
        }
	
        public IMediaItemRepository MediaItemRepository
        {
            get
            {
                if (mediaItemRepository == null)
                {
                    mediaItemRepository = new SqliteMediaItemRepository(connection);
                }
                return mediaItemRepository;
            }
        }

        #endregion
    };
}
