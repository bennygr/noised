using System.Collections.Generic;
namespace Noised.Core.Plugins
{
	/// <summary>
	///		A plugin loader for loading installed plugins
	/// </summary>
	public interface IPluginLoader
	{
		/// <summary>
		///		Loads all plugins from a local directory
		/// </summary>
		/// <param name="localPluginPath">The path to the local directory</param>
		int LoadPlugins(string localPluginPath);
	
		/// <summary>
		///		Gets all plugins
		/// </summary>
		/// <returns>Returns all loaded plugins</returns>
		IEnumerable<IPlugin> GetPlugins();

		/// <summary>
		///		Gets all plugins of a certain type
		/// </summary>
		/// <returns>Returns all loaded plugins of type T</returns>
		IEnumerable<T> GetPlugins<T>();

		/// <summary>
		///		Gets the first plugins of a certain type
		/// </summary>
		/// <returns>Returns all loaded plugins of type T</returns>
		T GetPlugin<T>();
	};
}
