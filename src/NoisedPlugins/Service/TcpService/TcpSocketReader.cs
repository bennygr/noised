using System;
using System.Net.Sockets;

namespace Noised.Plugins.Service.TcpService
{
	/// <summary>
	///		A helper class for reading from a TCP socket
	/// </summary>
    internal class TcpSocketReader
    {
		#region Fields
		
		private readonly Socket socket;
		
		#endregion

		#region Constructor
		
		/// <summary>
		///		Constructor
		/// </summary>
		/// <param name="socket">The socket to read from</param>
		internal TcpSocketReader(Socket socket)
		{
		    this.socket = socket;
		}
		
		#endregion

		#region Methods
		
		/// <summary>
		///		Reads the next chunf of data from the socket
		/// </summary>
		internal byte[] ReadNext()
		{
		    try
		    {
		        byte[] buffer = new byte[5000*1024];
		        int len = socket.Receive(buffer);
		        if (len == 0)
		        {
		            //Disconnect
		            return null;
		        }
		        return buffer;
		    }            
		    catch (Exception)
		    {
		        //Disconnect
		        return null;                   
		    }
		}
		
		#endregion
    }
}
