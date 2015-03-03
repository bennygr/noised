using System;
using System.Net.Sockets;

namespace Noised.Network.Services.TcpService
{
	/// <summary>
	///		Client connecting to a TCP/IP service
	/// </summary>
    public class TcpNetworkClient : INetworkClient
    {
		#region Fields
		
		private readonly string hostIp;
		private readonly int port;
		private System.Net.Sockets.TcpClient client;
		private TcpConnection connection;
		
		#endregion
		/// <param name="hostIp">The IP of the host to connect to</param>
		/// <param name="port"> The port to connect to</param>
		TcpNetworkClient(string hostIp, int port)
		{
			this.hostIp = hostIp;
			this.port = port;
		}

        #region Methods

		/// <summary>
		///		Internal method to invoke the connected event
		/// </summary>
        private void InvokeConnected(NetworkEventArgs eventargs)
        {
            NetworkEventHandler handler = Connected;
            if (handler != null) handler(this, eventargs);
        }

        #endregion

        #region INetworkClient

        public event NetworkEventHandler Connected;
        public event NetworkEventHandler Disconnected;

        public void Connect()
        {
            client = new System.Net.Sockets.TcpClient(hostIp, port);
            connection = new TcpConnection(client);
            connection.Closed += Disconnected;
            connection.Start();
            InvokeConnected(new NetworkEventArgs(connection,data: null));
        }

        public void Disconnect()
        {
            connection.Close();
        }

        #endregion
    }
}
