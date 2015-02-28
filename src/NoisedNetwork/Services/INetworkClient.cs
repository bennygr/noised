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
		/// <param name="hostIp">The IP of the host to connect to</param>
		/// <param name="port"> The port to connect to</param>
		void Connect(string hostIp, int port);

		/// <summary>
		///		Disconnects from a noised service
		/// </summary>
		void Disconnect();
	};

}
