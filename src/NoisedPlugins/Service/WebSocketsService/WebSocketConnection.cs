using System.Text;
using Fleck;
using Noised.Core.Plugins.Service;
using Noised.Core.Service;

namespace Noised.Plugins.Service.WebSocketsService
{
    /// <summary>
    ///		A websocket connection
    /// </summary>
    public class WebSocketConnection : IServiceConnection
    {
        private WebSocketsServicePlugin service;
        private IWebSocketConnection connection;

        /// <summary>
        ///		Constructor
        /// </summary>
        /// <param name="service">The websocket service related to this connection</param>
        internal WebSocketConnection(WebSocketsServicePlugin service,
                                      IWebSocketConnection connection)
        {
            this.service = service;
            this.connection = connection;
            connection.OnMessage += OnReceive;
            connection.OnClose += InvokeOnClosed;
        }

        private void OnReceive(string message)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(message);
            InvokeOnDataReceived(bytes);
        }

        /// <summary>
        ///		Internal method to invoke the DataReceived event
        /// </summary>
        private void InvokeOnDataReceived(byte[] bytesRed)
        {
            ServiceEventHandler handler = DataReceived;
            if (handler != null) handler(this, new ServiceEventArgs(this, bytesRed));
        }

        /// <summary>
        ///		Internal method to invoke the Closed event
        /// </summary>
        private void InvokeOnClosed()
        {
            ServiceEventHandler handler = Closed;
            if (handler != null) handler(this, new ServiceEventArgs(this, data: null));
        }

        #region IServiceConnection

        public event ServiceEventHandler DataReceived;
        public event ServiceEventHandler Closed;

        public IService Service
        {
            get
            {
                return this.service;
            }
        }

        public void Send(byte[] bytes)
        {
            string message = Encoding.UTF8.GetString(bytes);
            this.connection.Send(message);
        }

        public void Close()
        {
            this.connection.Close();
        }

        #endregion
    };
}
