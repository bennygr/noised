using System;
using System.Net.Sockets;
using System.Collections.Generic;

namespace Noised.Network.Services.TcpService
{
	/// <summary>
	///		A helper class for writing to a TCP socket
	/// </summary>
    internal class TcpSocketWriter
    {
		#region Fields
		
		private readonly Socket socket;
		private readonly Queue<byte[]> queue;
		
		#endregion

		#region Properties
		
		/// <summary>
		///		The current size of the queue
		/// </summary>
		internal int QueueSize
		{
		    get
		    {
		        lock(queue)
		        {
		            return queue.Count;
		        }
		    }
		}
		
		#endregion

		#region Constructor
		
		/// <summary>
		///		Constructor
		/// </summary>
		/// <param name="socket"> The socket to write to</param>
		internal TcpSocketWriter(Socket socket)
		{
		    this.socket = socket;
		    this.queue = new Queue<byte[]>();
		}
		
		#endregion

		#region Methods
		
		/// <summary>
		///		Adds some bytes to the send-queue
		/// </summary>
		/// <param name="bytes">bytes to add</param>
		internal void AddToQueue(byte[] bytes)
		{
		    lock(queue)
		    {
		        this.queue.Enqueue(bytes);
		    }
		}
		
		/// <summary>
		/// Sends the next ByteArray in the queue
		/// </summary>
		/// <returns>The amount of bytes sent</returns>
		internal int SendNext()
		{
		    byte[] packageToSend = null;
		    lock(queue)
		    {
		        if(queue.Count > 0)
		            packageToSend = queue.Dequeue();
		    }
		    if(packageToSend != null)
		    {
		        try
		        {
		            return this.socket.Send(packageToSend);
		        }
		        catch(Exception)
		        {
		            return 0;
		        }
		    }
		    return 0;
		}
		
		#endregion
    }
}
