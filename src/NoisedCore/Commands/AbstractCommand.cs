using System;
using Noised.Logging;
using Noised.Core.IOC;
using Noised.Core.Service;
namespace Noised.Core.Commands
{
	/// <summary>
	///		An abstract noised command
	/// </summary>
	/// <remarks>
	///		Extend this class in order to define your own commands
	/// </remarks>
	public abstract class AbstractCommand
	{
		#region Fields
		
		private ILogging logging;
		
		#endregion

		#region Properties
		
		public ServiceConnectionContext Context{get; private set;}

		#endregion

		#region Constructor
	
		/// <summary>
		///		Constructor
		/// </summary>
		/// <param name="context">The command's context</param>
		protected AbstractCommand (ServiceConnectionContext context)
		{
			logging = IocContainer.Get<ILogging>();
			this.Context = context;
		}
	
		#endregion

		#region Methods

		/// <summary>
		///		Defines the command's behaviour
		/// </summary>
		protected abstract void Execute();
		
		/// <summary>
		///		Executes the command
		/// </summary>
		public void ExecuteCommand()
		{
			try
			{
				Execute();
			}
			catch(Exception e)
			{
				//TODO: Send error to the user
				
				string errorMessage = 
					e + 
					Environment.NewLine + 
					e.Message +
					Environment.NewLine + 
					e.StackTrace;
				logging.Error(errorMessage);
			}
		}
		
		#endregion
	};
}
