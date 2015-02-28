using System;

namespace Noised.Network.Services
{
	/// <summary>
	///		Eventargs used in Noised.Network events
	/// </summary>
    public class NetworkEventArgs : EventArgs
    {
		/// <summary>
		///		The connection related to the network event
		/// </summary>
        public INetworkConnection Connection { get; set; }

		/// <summary>
		///		data related to the event
		/// </summary>
        public byte[] Data { get; set; }

		/// <summary>
		///		Constructor
		/// </summary>
		/// <param name="connection">
		///		The connection related to the network event	
		/// </param>
		/// <param name="data">Data read/sent</param>
        internal NetworkEventArgs(INetworkConnection connection, byte[] data)
        {
            Connection = connection;
			Data = data;
        }
    }
}
