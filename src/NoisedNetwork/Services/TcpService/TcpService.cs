using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;
using Noised.Logging;

namespace Noised.Network.Services.TcpService
{
    /// <summary>
	///		Service handling incoming connections over TCP/IP
    /// </summary>
    public class TcpService : INetworkService
    {
        #region Fields

        private TcpListener listener;
        private Thread mainThread;
        private readonly int port;
        private bool isRunning;
        private readonly List<TcpClient> clients = new List<TcpClient>();
        private readonly int maxConnections;
        private IPAddress listeningAddress = IPAddress.Any;
		private ILogging logging;

        #endregion

		#region Constructor
		
		/// <summary>
		///		Constructor
		/// </summary>
		/// <param name="logging">logging service</param>        
		/// <param name="port"> The Server's listening port </param>        
		/// <param name="maxConnections">
		///		Maxium of connection the server should handle
		/// </param>
		public TcpService(ILogging logging, int port, int maxConnections)
		{
			this.logging = logging;
		    this.port = port;
		    this.maxConnections = maxConnections;                     
		    isRunning = false;            
		}
		
		#endregion

		#region Methods
		
		/// <summary>
		///		Inkoves the ClientConnected event
		/// </summary>
        private void InvokeOnClientConnected(NetworkEventArgs eventargs)
        {
            NetworkEventHandler handler = ClientConnected;
            if (handler != null) handler(this, eventargs);
        }

		/// <summary>
		///		Invokes the ClientDisconnected event
		/// </summary>
        private void InvokeOnClientDisconnected(NetworkEventArgs eventargs)
        {
            NetworkEventHandler handler = ClientDisconnected;
            if (handler != null) handler(this, eventargs);
        }
		
		/// <summary>
        ///		Internal method which listens for connection requests and starts new communication threads.
		/// </summary>
        private void Run()
        {
			logging.Debug("Listening for incoming connections");
            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();

                if(maxConnections > 0 &&
                   clients.Count >= maxConnections)
                {
                    client.Client.Close();
                    logging.Debug("Client tried to connect but MaxConnections has been reached");
                    continue;
                }

                TcpConnection connection = new TcpConnection(client);
                InvokeOnClientConnected(new NetworkEventArgs(connection,data: null));
                connection.Closed += OnConnectionClosed;
                AddConnection(connection);
                connection.Start();
            }
        }

        /// <summary>
        ///		Internal Method to handle closing sockets
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="args">parameters</param>
		private void OnConnectionClosed(object sender, NetworkEventArgs eventArgs)
        {
            INetworkConnection connection = (INetworkConnection)sender;
            RemoveClient((TcpConnection)connection);
            InvokeOnClientDisconnected(new NetworkEventArgs(connection,data: null));
        }
		
        /// <summary>
        ///		Internal method to add a client connection to the list of known connections
        /// </summary>
        /// <param name="connection">the connection client to add</param>        
        private void AddConnection(TcpConnection connection)
        {
            lock (clients)
            {                
                clients.Add(connection.Client);
                logging.Debug("Client " + 
									 connection.Id + 
									 " connected " + 
									 ((IPEndPoint)connection.Client.Client.RemoteEndPoint).Address);
            }
        }

        /// <summary>
        ///		Internal Method to remove a client connection from the list of known connections
        /// </summary>
        /// <param name="connection">the connection to remove</param>        
        private void RemoveClient(TcpConnection connection)
        {
            lock (clients)
            {
                clients.Remove(connection.Client);
                logging.Debug("Client " + connection.Id + " disconnected");
            }
        }

		#endregion

        #region INetworkService

		public event NetworkEventHandler ClientConnected;
		public event NetworkEventHandler ClientDisconnected;

        public bool IsRunning
        {
            get { return isRunning; }
        }


        public void Start()
        {
            listener = new TcpListener(listeningAddress, this.port);             
            listener.Start();            
            mainThread = new Thread(Run);
            mainThread.IsBackground = true;
            mainThread.Start();
            isRunning = true;
        }
		
        public void Stop()
        {
            mainThread.Abort();
            listener.Stop();
            foreach (TcpClient client in clients)
            {
                client.Client.Close();
            }
            isRunning = false;
        }

        #endregion
    }
}
