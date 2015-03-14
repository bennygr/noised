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
		///		Enqueues a command to be executed
		/// </summary>
		void AddCommand(AbstractCommand command);
	};
}
