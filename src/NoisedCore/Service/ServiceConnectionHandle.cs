using System;
using Noised.Core.IOC;

namespace Noised.Core.Service
{
	/// <summary>
	///		A handle which wrapps a IServiceConnection at a higher level
	/// </summary>
	internal class ServiceConnectionHandle
	{
		#region Fields
		
		private CommandDataBuffer commandBuffer;
		
		#endregion

		#region Properties

		/// <summary>
		///		The service connection to handle
		/// </summary>
		internal IServiceConnection Connection{get; private set;}

		#endregion

		#region Constructor

		/// <summary>
		///		Constructor
		/// </summary>
		/// <param name="connection">The service connection to handle</param>
		internal ServiceConnectionHandle(IServiceConnection connection)
		{
			connection.DataReceived += DataReceived;
			this.Connection = connection;
			this.commandBuffer = new CommandDataBuffer();
		}

		#endregion

		#region Methods
		
		/// <summary>
		///		Method to handle received data from the connection
		/// </summary>
		private void DataReceived(object sender,ServiceEventArgs eventArgs)
		{
			if(commandBuffer.Add(eventArgs.Data))
			{
				string command = String.Empty;
				while((command = commandBuffer.PopCommand()) != string.Empty)
					Console.WriteLine(command);
			}
		}
		
		#endregion
	};
}
