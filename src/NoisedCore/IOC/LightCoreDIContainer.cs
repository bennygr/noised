using LightCore;
using LightCore.Lifecycle;
using Noised.Core.Commands;
using Noised.Core.Config;
using Noised.Core.Config.Filesystem;
using Noised.Core.Crypto;
using Noised.Core.DB;
using Noised.Core.DB.Sqlite;
using Noised.Core.Media;
using Noised.Core.Plugins;
using Noised.Core.Service;
using Noised.Core.Service.Protocols;
using Noised.Core.Service.Protocols.JSON;
using Noised.Core.UserManagement;
using Noised.Logging;

namespace Noised.Core.IOC
{
    /// <summary>
    ///	    DI Container implementation using LightCore (http://lightcore.ch/)
    /// </summary>
    class LightCoreDIContainer : IDIContainer
    {
        private IContainer container;

        #region IDIContainer implementation

        public void Build()
        {
            var builder = new ContainerBuilder();

            //Logging
            builder.Register<ILogging, Logger>().
                ControlledBy<SingletonLifecycle>();

            //Configuration
            builder.Register<IConfigurationLoader, FilesystemConfigurationLoader>();
            builder.Register<IConfig, Config.Config>().
                ControlledBy<SingletonLifecycle>();

            //DB
            builder.Register<IDB, SqliteDB>();
            builder.Register<IUnitOfWork, SqliteUnitOfWork>();
            builder.Register<IDbFactory, DbFactory>();

            //The core
            builder.Register<ICore, Core>().
                ControlledBy<SingletonLifecycle>();

            //Connection handling
            builder.Register<IServiceConnectionManager, ServiceConnectionManager>().
                ControlledBy<SingletonLifecycle>();

            //Plugins
            builder.Register<IPluginInstaller, PluginInstaller>();
            builder.Register<IPluginLoader, PluginLoader>().
                ControlledBy<SingletonLifecycle>();

            //Protocol
            builder.Register<IProtocol, JSONProtocol>();
            builder.Register<ICommandFactory, CommandFactory>().
                ControlledBy<SingletonLifecycle>();

            //Media
            builder.Register<IMediaSourceAccumulator, MediaSourceAccumulator>().
                ControlledBy<SingletonLifecycle>();
            builder.Register<IMediaManager, MediaManager>().
                ControlledBy<SingletonLifecycle>();
            builder.Register<IMetaFileAccumulator, MetaFileAccumulator>().ControlledBy<SingletonLifecycle>();
            builder.Register<IMetaFileWriter, MetaFileWriter>().ControlledBy<SingletonLifecycle>();

            //Crypto
            builder.Register<IChecksum, MD5Checksum>();

            // PlaylistManager
            builder.Register<IPlaylistManager, PlaylistManager>().ControlledBy<SingletonLifecycle>();

            //Queue
            builder.Register<IQueue, Queue>().ControlledBy<SingletonLifecycle>();

            // UserManager
            builder.Register<IUserManager, UserManager>().ControlledBy<SingletonLifecycle>();

            container = builder.Build();
        }

        public T Get<T>()
        {
            return container.Resolve<T>();
        }

        #endregion
    };
}
