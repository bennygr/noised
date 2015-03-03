using Noised.Logging;
namespace Noised.Network.Services
{
	/// <summary>
	///		A handler used in INetworkService events 
	/// </summary>
	/// <param name="sender">The sender of the event</param>
	/// <param name="eventArgs">The event arguments</param>
	public delegate void NetworkEventHandler(object sender, NetworkEventArgs eventArgs);

	/// <summary>
	///		A service providing network access
	/// </summary>
	public interface INetworkService
	{
		#region Events
		
		/// <summary>
		///		Fired if a client  connects to the service
		/// </summary>
		event NetworkEventHandler ClientConnected;

		/// <summary>
		///		FIred if a client disconnects from the service
		/// </summary>
		event NetworkEventHandler ClientDisconnected;
		
		#endregion

		#region Properties
		
		/// <summary>
		///		Whether the Service is running or not
		/// </summary>
		bool IsRunning{get;}
		
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
	};
}
