using System;

namespace Noised.Core.Plugins.Service
{
	/// <summary>
	///		Eventargs used for IService events
	/// </summary>
    public class ServiceEventArgs : EventArgs
    {
		/// <summary>
		///		The connection related to the service event
		/// </summary>
        public IServiceConnection Connection { get; set; }

		/// <summary>
		///		data related to the event
		/// </summary>
        public byte[] Data { get; set; }

		/// <summary>
		///		Constructor
		/// </summary>
		/// <param name="connection">
		///		The connection related to the service event	
		/// </param>
		/// <param name="data">Data read/sent</param>
        internal ServiceEventArgs(IServiceConnection connection, byte[] data)
        {
            Connection = connection;
			Data = data;
        }
    }
}
