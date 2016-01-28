using Noised.Core.Commands;

namespace Noised.Core.Service
{
	/// <summary>
	///		Connection Manager
	/// </summary>
	public interface IServiceConnectionManager
	{
		/// <summary>
		///		Starts all known servives
		/// </summary>
		void StartServices();

		/// <summary>
		///		Send a response to all connected clients
		/// </summary>
		/// <param name="response">The response</param>
		void SendBroadcast(ResponseMetaData response);
	};
}
