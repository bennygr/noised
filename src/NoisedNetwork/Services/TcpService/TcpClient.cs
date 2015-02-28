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
		
		private System.Net.Sockets.TcpClient client;
		private TcpConnection connection;
		
		#endregion

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

        public void Connect(string hostIp, int port)
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
