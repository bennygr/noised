using System.Collections.Generic;
using System.Threading;
using Noised.Core.Commands;
using Noised.Logging;
using Noised.Core.IOC;

namespace Noised.Core
{
	/// <summary>
	///		The noised core processing all incoming commands
	/// </summary>
	public class Core
	{
		#region Fields
		
		private readonly Queue<AbstractCommand> commandQueue;
		private bool isRunning;
		private ILogging logging;
		
		#endregion

		#region Constructor
		
		/// <summary>
		///		Constructor
		/// </summary>
		public Core()
		{
			commandQueue = new Queue<AbstractCommand>();
			logging = IocContainer.Get<ILogging>();
		}
		
		#endregion
	
		#region Properties
		
		/// <summary>
		///		Whether the core is running or not
		/// </summary>
		private bool IsRunning
		{
			get
			{
				lock(this)
				{
					return isRunning;
				}
			}
		}
		
		#endregion

		#region Methods
		
		/// <summary>
		///		Internal method to process the core's command queue
		/// </summary>
		private void ProcessCommands()
		{
			isRunning = true;
			logging.Debug("noised core started");
			while(isRunning)
			{
				AbstractCommand cmd = null;
				lock(commandQueue)
				{
					if(commandQueue.Count > 0)
						cmd = commandQueue.Dequeue();
				}

				if(cmd != null)
				{
					cmd.ExecuteCommand();
					logging.Debug("Executed: " + cmd);
				}

				Thread.Sleep(1);
			}
			logging.Debug("noised core stopped");
		}

		/// <summary>
		///		Starts the noised core
		/// </summary>
		public void Start()
		{
			lock(this)
			{
				if(isRunning)
					throw new CoreException(
						"Cannot start noised Core because it's still running");

				Thread coreThread = new Thread(ProcessCommands);
				coreThread.Start();
				logging.Debug("Starting noised core...");
			}
		}

		
		/// <summary>
		///		Stops the core
		/// </summary>
		public void Stop()
		{
			lock(this)
			{
				isRunning = false;
				logging.Debug("Stopping noised core...");
			}
		}

		/// <summary>
		///		Enqueues a command to be executed
		/// </summary>
		public void AddCommand(AbstractCommand command)
		{
			lock(commandQueue)	
			{
				commandQueue.Enqueue(command);
			}
		}
		
		#endregion
	};
}
