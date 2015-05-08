namespace Noised.Core.Plugins.Commands
{
	/// <summary>
	///		Interface defining a command bundle plugin
	/// </summary>
	/// <remarks>
	///		A command bundle plugin is a plugin which 
	///		provides a bunch of commands. This is just
	///		a marker interface. Commands are created
	///		using reflection.
	///	</remarks>
	public interface ICommandBundle : IPlugin { };
}
