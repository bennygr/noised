using System;
using Noised.Logging;
using Noised.Core.IOC;
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
		
		/// <summary>
		///		The user which invokes the command
		/// </summary>
		public User User {get; private set;}

		#endregion

		#region Constructor
	
		/// <summary>
		///		Constructor
		/// </summary>
		/// <param name="user">The user which invokes the command</param>
		protected AbstractCommand (User user)
		{
			this.User = user;
			logging = IocContainer.Get<ILogging>();
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
