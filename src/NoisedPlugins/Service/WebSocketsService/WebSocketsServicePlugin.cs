using System;
using System.Collections.Generic;
using Fleck;
using Noised.Core.Plugins;
using Noised.Core.Plugins.Service;
using Noised.Core.Service;
using Noised.Logging;

namespace Noised.Plugins.Service.WebSocketsService
{
    /// <summary>
    ///		A service providing a websocket server
    /// </summary>
    public class WebSocketsServicePlugin : IService
    {
        private bool isRunning = false;
        private WebSocketServer server;
        private List<WebSocketConnection> connections;

        /// <summary>
        ///		Constructor
        /// </summary>
        /// <param name="initalizer">initalizting data</param>        
        public WebSocketsServicePlugin(PluginInitializer initalizer)
        {
            server = new WebSocketServer("ws://0.0.0.0:1338");
            connections = new List<WebSocketConnection>();
        }

        #region IDisposable

        public void Dispose() { }

        #endregion

        #region methods

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
        ///		Internal Method to handle closing sockets
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="args">parameters</param>
        private void OnConnectionClosed(object sender, ServiceEventArgs eventArgs)
        {
            IServiceConnection connection = (IServiceConnection)sender;
            RemoveClient((WebSocketConnection)connection);
            InvokeOnClientDisconnected(new ServiceEventArgs(connection, data: null));
        }

        /// <summary>
        ///		Internal Method to remove a client connection from the list of known connections
        /// </summary>
        /// <param name="connection">the connection to remove</param>        
        private void RemoveClient(WebSocketConnection connection)
        {
            lock (connections)
            {
                connections.Remove(connection);
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
            server.Start(
                socket =>
                {
                    socket.OnOpen = () =>
                        {
                            WebSocketConnection connection =
                                new WebSocketConnection(this, socket);
                            connections.Add(connection);
                            connection.Closed += OnConnectionClosed;
                            InvokeOnClientConnected(new ServiceEventArgs(connection, data: null));
                        };
                }
            );
            isRunning = true;
        }

        public void Stop()
        {
            isRunning = false;
        }

        #endregion
    };
}
