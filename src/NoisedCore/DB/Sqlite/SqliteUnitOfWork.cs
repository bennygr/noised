using Mono.Data.Sqlite;

namespace Noised.Core.DB.Sqlite
{
    /// <summary>
    ///		Sqlite implementation of IUnitOfWork
    /// </summary>
    public class SqliteUnitOfWork : IUnitOfWork
    {
        private IPluginRepository pluginRepository;
        private IPlaylistRepository playlistRepository;
        private readonly SqliteConnection connection;
        private readonly SqliteTransaction transaction;

        public SqliteUnitOfWork()
        {
            var connectionString = "Data Source=" + SqliteFileSource.GetDBFileName();
            connection = new SqliteConnection(connectionString);
            connection.Open();
            transaction = connection.BeginTransaction();
        }

        #region IUnitOfWork implementation

        public IPluginRepository PluginRepository
        {
            get
            {
                if (pluginRepository == null)
                {
                    pluginRepository = new SqlitePluginRepository(connection);
                }
                return pluginRepository;
            }
        }

        /// <summary>
        /// Repository for Playlists
        /// </summary>
        public IPlaylistRepository PlaylistRepository
        {
            get
            {
                if (playlistRepository == null)
                    playlistRepository = new SqlitePlaylistRepository(connection, this);
                return playlistRepository;
            }
        }

        public void SaveChanges()
        {
            transaction.Commit();
        }

        #endregion

        #region IDisposable implementation

        public void Dispose()
        {
            if (connection != null)
            {
                connection.Close();
            }
        }

        #endregion
    };
}
