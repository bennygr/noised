using LightCore;
using LightCore.Lifecycle;
using Noised.Logging;
using Noised.Core;
using Noised.Core.Commands;
using Noised.Core.Config;
using Noised.Core.Config.Filesystem;
using Noised.Core.Media;
using Noised.Core.Plugins;
using Noised.Core.Service.Protocols;
using Noised.Core.Service.Protocols.JSON;
using Noised.Core.DB;
using Noised.Core.DB.Sqlite;

namespace Noised.Core.IOC
{
	/// <summary>
	///		Noised core depedency injection container
	/// </summary>
	public static class IocContainer
	{
		#region Fields
	
		private static IContainer container; 
	
		#endregion
	
		#region Methods
	
		/// <summary>
		///		Creates the dependency injection-container
		/// </summary>
		public static void Build()
		{
			var builder = new ContainerBuilder();
	
			//Logging
			builder.Register<ILogging,Logger>().
				ControlledBy<SingletonLifecycle>();
			//Configuration
			builder.Register<IConfigurationLoader,FilesystemConfigurationLoader>();
			builder.Register<IConfig,Config.Config>();
			//DB
			builder.Register<IDB,SqliteDB>();
			builder.Register<IUnitOfWork,SqliteUnitOfWork>();
			builder.Register<IPluginRepository,SqlitePluginRepository>();
			//The core
			builder.Register<ICore,Core>().
				ControlledBy<SingletonLifecycle>();
			//Plugins
			builder.Register<IPluginInstaller,PluginInstaller>();
			builder.Register<IPluginLoader,PluginLoader>().
				ControlledBy<SingletonLifecycle>();
			//Protocol
			builder.Register<IProtocol,JSONProtocol>();
			builder.Register<ICommandFactory,CommandFactory>().
				ControlledBy<SingletonLifecycle>();
			//Media
			builder.Register<IMediaSourceAccumulator,MediaSourceAccumulator>().
				ControlledBy<SingletonLifecycle>();
			builder.Register<IMediaManager,MediaManager>().
				ControlledBy<SingletonLifecycle>();
	
			container = builder.Build();
		}
	
		/// <summary>
		///		Get the Service for type T
		/// </summary>
		/// <return>The service fot type T</return>
		public static T Get<T>()
		{
			return container.Resolve<T>();
		}
	
		#endregion
	};
}
