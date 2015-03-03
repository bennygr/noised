namespace Noised.Network.Services
{
	/// <summary>
	///		Represents a client which connects to a remote noised service
	/// </summary>
	interface INetworkClient
	{
		/// <summary>
		///		Will be invoked if the client connected to the remote service
		/// </summary>
        event NetworkEventHandler Connected;

		/// <summary>
		///		Will be invoked if the client was disconnected from the remote service
		/// </summary>
        event NetworkEventHandler Disconnected;

		/// <summary>
		///		Connects to a noised service
		/// </summary>
		void Connect();

		/// <summary>
		///		Disconnects from a noised service
		/// </summary>
		void Disconnect();
	};

}
