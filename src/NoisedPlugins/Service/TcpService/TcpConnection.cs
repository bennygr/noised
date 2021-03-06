﻿using System.Net.Sockets;
using System.Threading;
using Noised.Core.Plugins.Service;
using Noised.Core.Service;

namespace Noised.Plugins.Service.TcpService
{
    /// <summary>
    ///		A TCP connection to a client
    /// </summary>
    public class TcpConnection : IServiceConnection
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
        private readonly TcpServicePlugin service;
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
        /// <param name="service">The service related to this connection</param>
        internal TcpConnection(TcpClient client, TcpServicePlugin service)
        {
            lock (IdLock)
            {
                id = idCounter++;
            }
            this.client = client;
            this.service = service;
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
            while (running)
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
            ServiceEventHandler handler = DataReceived;
            if (handler != null) handler(this, new ServiceEventArgs(this, bytesRed));
        }

        /// <summary>
        ///		Internal method to invoke the closed event
        /// </summary>
        private void InvokeOnClose()
        {
            ServiceEventHandler handler = Closed;
            if (handler != null) handler(this, new ServiceEventArgs(this, data: null));
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
