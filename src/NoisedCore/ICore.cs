using Noised.Core.Commands;

namespace Noised.Core
{
	/// <summary>
	///		The noised core
	/// </summary>
	public interface ICore
	{
		/// <summary>
		///		Whether the core is running or not
		/// </summary>
		bool IsRunning{get;}

		/// <summary>
		///		Starts the noised core
		/// </summary>
		void Start();
		
		/// <summary>
		///		Stops the core
		/// </summary>
		void Stop();
		
		/// <summary>
		///		Executes a command 
		/// </summary>
		void ExecuteCommand(AbstractCommand command);
		
		/// <summary>
		///		Executes a command asynchronous
		/// </summary>
		void ExecuteCommandAsync(AbstractCommand command);
	};
}
