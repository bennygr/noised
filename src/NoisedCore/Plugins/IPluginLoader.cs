using System.Collections.Generic;
namespace Noised.Core.Plugins
{
	/// <summary>
	///		A plugin loader for loading installed plugins
	/// </summary>
	public interface IPluginLoader
	{
		/// <summary>
		///		Loads all known plugins
		/// </summary>
		int LoadPlugins();
	
		/// <summary>
		///		Gets all plugins
		/// </summary>
		/// <returns>Returns all loaded plugins</returns>
		IEnumerable<IPlugin> GetPlugins();

		/// <summary>
		///		Gets all plugins of a certain type
		/// </summary>
		/// <returns>Returns all loaded plugins of type T</returns>
		IEnumerable<T> GetPlugins<T>() where T : IPlugin;

		/// <summary>
		///		Gets the plugin with the highest priority of a certain type 
		/// </summary>
		/// <returns>Returns all loaded plugins of type T</returns>
		T GetPlugin<T>() where T : IPlugin;

		/// <summary>
		///		Gets the meta data for a given plugin
		/// </summary>
		/// <param name="plugin">The plugin to get the meta data for</param>
		PluginMetaData GetMetaData(IPlugin plugin);
	};
}
