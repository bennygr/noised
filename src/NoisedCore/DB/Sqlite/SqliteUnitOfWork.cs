using Mono.Data.Sqlite;
using Noised.Core.DB;

namespace Noised.Core.DB.Sqlite
{
    public class SqliteUnitOfWork : IUnitOfWork
    {
		private IMediaItemRepository mediaItemRepository;
		private IPluginRepository pluginRepository;
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

		public IMediaItemRepository MediaItemRepository
		{
			get
			{
				if(mediaItemRepository == null)
				{
					mediaItemRepository = new SqliteMediaItemRepository(connection);
				}
				return mediaItemRepository;
			}
		}

		public IPluginRepository PluginRepository
		{
			get
			{
				if(pluginRepository == null)
				{
					pluginRepository = new SqlitePluginRepository(connection);
				}
				return pluginRepository;
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
			if(connection != null)
			{
				connection.Close();
			}
        }

        #endregion
    };
}
