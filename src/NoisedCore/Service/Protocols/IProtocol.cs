using Noised.Core.Commands;
namespace Noised.Core.Service.Protocols
{
	/// <summary>
	///		Defines which protocol to use for sending commands
	/// </summary>
	public interface IProtocol
	{
		/// <summary>
		///		Defines an end tag for a command in a 
		///		stream of incoming command data
		/// </summary>
		string CommandEndTag{get;}

		/// <summary>
		///		Parses a command from incoming command data
		/// </summary>
		/// <param name="context">The command's context</param>
		/// <param name="commandData">The data to parse as command</param>
		AbstractCommand Parse(ServiceConnectionContext context, string commandData);
	};
}
