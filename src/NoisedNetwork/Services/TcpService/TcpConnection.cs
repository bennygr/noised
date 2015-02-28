using System;
using System.Net.Sockets;
using System.Threading;

namespace Noised.Network.Services.TcpService
{
    /// <summary>
    ///		A TCP connection to a client
    /// </summary>
    public class TcpConnection : INetworkConnection
    {
        #region Statics 

        private static readonly object IdLock = new object();
        private static int idCounter;

        #endregion

        #region Fields

        private readonly int id;
        private readonly TcpSocketReader socketReader;
        private readonly TcpSocketWriter socketWriter;
        private readonly Thread readerThread;
        private readonly Thread writerThread;
        private readonly TcpClient client;
        private bool running;
                
        #endregion

        #region Properties

        /// <summary>
        ///		TCP endpoint to the connected client
        /// </summary>
        internal TcpClient Client
        {
            get
            {
                return client;
            }
        }

        /// <summary>
        ///		A unique connection id
        /// </summary>
        internal int Id
        {
            get
            {
                return id;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        ///		Constructor
        /// </summary>
        /// <param name="client">TCP endpoint of the connected client</param>
        internal TcpConnection(TcpClient client)
        {
            lock(IdLock)
            {
                id = idCounter++;
            }
            this.client = client;
            socketReader = new TcpSocketReader(client.Client);
            socketWriter = new TcpSocketWriter(client.Client);
            readerThread = new Thread(Read);
            writerThread = new Thread(Write);
            readerThread.IsBackground = true;
            writerThread.IsBackground = true;
        }

        #endregion

        #region Methods

		/// <summary>
		///		Internal method wait for incoming data to read from the socket
		/// </summary>
        private void Read()
        {
            while(running)
            {
                byte[] bytes = socketReader.ReadNext();
                if (bytes != null)
                    InvokeOnDataReceived(bytes);
                else
                {
                    running = false;
                    InvokeOnClose();
                }
                Thread.Sleep(1);
            }
        }

		/// <summary>
		///		Internal method to send data to the connected client
		/// </summary>
        private void Write()
        {
            while (running)
            {
                socketWriter.SendNext();
                Thread.Sleep(1);
            }

            //Send the rest of the queue
            for (int i = 0; i <= socketWriter.QueueSize; i++)
            {
                socketWriter.SendNext();
            }
        }

		/// <summary>
		///		Internal method to invoke the DataReceived event
		/// </summary>
        private void InvokeOnDataReceived(byte[] bytesRed)
        {
            NetworkEventHandler handler = DataReceived;
            if (handler != null) handler(this, new NetworkEventArgs(this,bytesRed));
        }

		/// <summary>
		///		Internal method to invoke the closed event
		/// </summary>
        private void InvokeOnClose()
        {
            NetworkEventHandler handler = Closed;
            if (handler != null) handler(this, new NetworkEventArgs(this, data: null));
        }
        
        /// <summary>
        ///		Internal method to start reading and writing from/to the socket
        /// </summary>
        internal void Start()
        {
            running = true;
            readerThread.Start();
            writerThread.Start();
        }

        #endregion

        #region INetworkConnection

        public event NetworkEventHandler DataReceived;
        public event NetworkEventHandler Closed;

        public void Send(byte[] bytes)
        {
            socketWriter.AddToQueue(bytes);            
        }

        public void Close()
        {
            running = false;
            writerThread.Join();
            client.Client.Close();
        }

        #endregion
    }
}
