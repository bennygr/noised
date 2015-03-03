using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;
using Noised.Logging;
using Noised.Core.Plugins;
using Noised.Core.Plugins.Service;

namespace Noised.Plugins.Service.TcpService
{
    /// <summary>
	///		Service handling incoming connections over TCP/IP
    /// </summary>
    public class TcpServicePlugin : IService
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
		public TcpServicePlugin(PluginInitializer initalizer)
		{
			this.logging = initalizer.Logging;
		    this.port = 1337;
		    this.maxConnections = 100;                     
		    isRunning = false;            
		}
		
		#endregion

		#region Methods
		
		/// <summary>
		///		Inkoves the ClientConnected event
		/// </summary>
        private void InvokeOnClientConnected(ServiceEventArgs eventargs)
        {
            ServiceEventHandler handler = ClientConnected;
            if (handler != null) handler(this, eventargs);
        }

		/// <summary>
		///		Invokes the ClientDisconnected event
		/// </summary>
        private void InvokeOnClientDisconnected(ServiceEventArgs eventargs)
        {
            ServiceEventHandler handler = ClientDisconnected;
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
                InvokeOnClientConnected(new ServiceEventArgs(connection,data: null));
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
		private void OnConnectionClosed(object sender, ServiceEventArgs eventArgs)
        {
            IServiceConnection connection = (IServiceConnection)sender;
            RemoveClient((TcpConnection)connection);
            InvokeOnClientDisconnected(new ServiceEventArgs(connection,data: null));
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


		#region IDisposable

		public void Dispose(){}

		#endregion
		
		#region IPlugin
		
		public String Name
		{
			get
			{
				return  "TcpServicePlugin";
			}
		}

		public String Description
		{
			get
			{
				return  "The plugin provides a TCP/IP service for noised";
			}
		}
		
		public String AuthorName
		{
			get
			{
				return "Benjamin Grüdelbach";
			}
		}

		public String AuthorContact
		{
			get
			{
				return "nocontact@availlable.de";
			}
		}

		public Version Version
		{
			get
			{
				return new Version(1,0);
			}
		}

		public DateTime CreationDate
		{
			get
			{
				return DateTime.Parse("01.03.2015");
			}
		}
		
		
		#endregion

        #region IService

		public event ServiceEventHandler ClientConnected;
		public event ServiceEventHandler ClientDisconnected;

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
