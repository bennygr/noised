using System;
using Noised.Logging;
using Noised.Core.Commands;
using Noised.Core.IOC;
using Noised.Core.Service.Protocols;

namespace Noised.Core.Service
{
	/// <summary>
	///		A handle which wrapps a IServiceConnection at a higher level
	/// </summary>
	internal class ServiceConnectionHandle
	{
		#region Fields
		
		private CommandDataBuffer commandBuffer;
		private IProtocol protocol;
		private ILogging logging; 
		
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
		/// <param name="protocol">noised protocol definition</param>
		internal ServiceConnectionHandle(IServiceConnection connection,IProtocol protocol)
		{
			connection.DataReceived += DataReceived;
			this.Connection = connection;
			this.commandBuffer = new CommandDataBuffer();
			this.protocol = protocol;
			this.logging = IocContainer.Get<ILogging>();
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
				string commandText = String.Empty;
				while((commandText = commandBuffer.PopCommand()) != string.Empty)
				{
					try
					{
					AbstractCommand command =  protocol.Parse(commandText);
						if(command != null)
							Console.WriteLine(command);
						else 
							Console.WriteLine("ERROR NULL command");
					}
					catch(Exception e)
					{
						logging.Error(e.Message);
					}
				}
			}
		}
		
		#endregion
	};
}
