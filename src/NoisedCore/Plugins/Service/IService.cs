using System;
using Noised.Core.Service;
namespace Noised.Core.Plugins.Service
{
	/// <summary>
	///		A handler used for IService events 
	/// </summary>
	/// <param name="sender">The sender of the event</param>
	/// <param name="eventArgs">The event arguments</param>
	public delegate void ServiceEventHandler(object sender, ServiceEventArgs eventArgs);

	/// <summary>
	///		A plugin for a a noised service
	/// </summary>
	/// <remarks>
	///		A noised service provides all noised functionality
	///		to a set of clients. A concrete implementation could be
	///		a TCP-Service, a Web-Socket-Service, a Bluetooth-Service...etc
	/// </remarks>
	public interface IService : IPlugin
	{
		#region Properties
		
		/// <summary>
		///		Whether the Service is running or not
		/// </summary>
		bool IsRunning{get;}
		
		#endregion

		#region events
		
		event ServiceEventHandler ClientConnected;
		event ServiceEventHandler ClientDisconnected;
		
		#endregion

		#region Methods
		
		/// <summary>
		///		Starts the network service
		/// </summary>
		void Start();

		/// <summary>
		///		Stops the network service
		/// </summary>
		void Stop();
		
		#endregion
	}
}
