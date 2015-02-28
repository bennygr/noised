namespace Noised.Network.Services
{
	/// <summary>
	///		A connection established to a network service INetworkService
	/// </summary>
	public interface INetworkConnection
	{
		#region Events
		
		/// <summary>
		///		Fired if data was received for the connection
		/// </summary>
		event NetworkEventHandler DataReceived;
		
		/// <summary>
		///		Fired if the connection has been closed
		/// </summary>
		event NetworkEventHandler Closed;
		
		#endregion

		#region Methods
		
		/// <summary>
		///		Send data
		/// </summary>
		/// <param name="data">The data to send</param>
		void Send(byte[] data);
		
		/// <summary>
		///		Closes the connection
		/// </summary>
		void Close();
		
		#endregion
	};
}
