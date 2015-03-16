using System;
using System.Linq;
using System.Collections.Generic;
using Noised.Logging;
using Noised.Core.IOC;
using Noised.Core.Plugins;
using Noised.Core.Plugins.Service;
using Noised.Core.Service.Protocols;

namespace Noised.Core.Service
{
	/// <summary>
	///		Handles incoming service connections
	/// </summary>
	public class ServiceConnectionManager
	{
		#region Fields
		
		private ILogging logging;
		private ICore core;
		private List<ServiceConnectionContext> connections;
		
		#endregion

		#region Constructor
		
		/// <summary>
		///		Constructor
		/// </summary>
		public ServiceConnectionManager()
		{
			this.logging = IocContainer.Get<ILogging>();
			this.core = IocContainer.Get<ICore>();
			this.connections = new List<ServiceConnectionContext>();
		}
		
		#endregion

		#region Methods

		/// <summary>
		///		Internal method to handle an incoming service connection
		/// </summary>
		private void ClientConnected(object sender,ServiceEventArgs eventArgs)
		{
			this.connections.Add(
				new ServiceConnectionContext(core,
											eventArgs.Connection,
											IocContainer.Get<IProtocol>()));
			logging.Debug("A new client connected.");
		}

		/// <summary>
		///		Internal method to handle a service disconnection
		/// </summary>
		private void ClientDisconnected(object sender,ServiceEventArgs eventArgs)
		{
			this.connections.RemoveAll(c => c.Connection == eventArgs.Connection);
			logging.Debug("A client disconnected");
		}

		/// <summary>
		///		Start all known noised services
		/// </summary>
		public void StartServices()
		{
			//Getting all loaded service plugins
			var services = IocContainer.Get<IPluginLoader>().GetPlugins<IService>();
			int serviceCount = services.Count();
			if(serviceCount > 0 )
			{
				logging.Debug("Starting " + serviceCount + " services...");
			}
			else
			{
				logging.Warning("No services found :-(.");
			}

			//Starting all services
			foreach(IService service in services)
			{
				try
				{
					logging.Debug("Starting service " + service.Name + "...");
					service.Start();
					service.ClientConnected += ClientConnected;
					service.ClientDisconnected += ClientDisconnected;
					logging.Debug("Service " + service.Name + " started");
				}
				catch(Exception e )
				{
					logging.Error("Error starting service " + service.Name + ": " + e.Message);
				}
			}
		}
		
		#endregion
	};
}
