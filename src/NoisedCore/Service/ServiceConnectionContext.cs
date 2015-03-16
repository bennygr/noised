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
	public class ServiceConnectionContext
	{
		#region Fields
		
		private CommandDataBuffer commandBuffer;
		private IProtocol protocol;
		private ILogging logging; 
		private ICore core;
		
		#endregion

		#region Properties

		/// <summary>
		///		The service connection to handle
		/// </summary>
		public IServiceConnection Connection{get; private set;}

		#endregion

		#region Constructor

		/// <summary>
		///		Constructor
		/// </summary>
		/// <param name="core">The core</param>
		/// <param name="connection">The service connection to handle</param>
		/// <param name="protocol">noised protocol definition</param>
		internal ServiceConnectionContext(ICore core, 
										  IServiceConnection connection,
										  IProtocol protocol)
		{
			connection.DataReceived += DataReceived;
			this.core = core;
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
						logging.Debug("Now creating command for:");
						logging.Debug(commandText);
						AbstractCommand command = protocol.Parse(this,commandText);
						if(command != null)
						{
							core.AddCommand(command);
						}
						else 
						{
							logging.Error("Error: Could not create command for text " + 
										  Environment.NewLine +
									      commandText);
						}
					}
					catch(Exception e)
					{
						logging.Error(e.Message);
						logging.Error(e.StackTrace);
					}
				}
			}
		}
		
		#endregion
	};
}
