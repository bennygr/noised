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
		#region Properties
		
		/// <summary>
		///		The contect in which the command is executed
		/// </summary>
		public ServiceConnectionContext Context{get; private set;}

		/// <summary>
		///		Whether the command requires authentication or not
		/// </summary>
		public bool RequiresAuthentication{get;protected set;}

		#endregion

		#region Constructor
	
		/// <summary>
		///		Constructor
		/// </summary>
		/// <param name="context">The command's context</param>
		/// <param name="requiresAuthentication">Whether the command requires authentication or not</param>
		protected AbstractCommand (ServiceConnectionContext context,
								   bool requiresAuthentication = true)
		{
			this.Context = context;
			this.RequiresAuthentication = requiresAuthentication;
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
			Execute();
		}
		
		#endregion
	};
}
