using System;
using Noised.Core.IOC;

namespace Noised.Core.Plugins
{
	/// <summary>
	///		A marker interface representing a plugin for extending noised
	/// </summary>
	public interface IPlugin : IDisposable { };

	/// <summary>
	///		Contains useful extension method for IPlugin
	/// </summary>
	public static class IPluginExtension
	{
		/// <summary>
		///		Extension method to get meta data for a given plugin
		/// </summary>
		public static PluginMetaData GetMetaData(this IPlugin plugin)
		{
			var pluginLoader = IoC.Get<IPluginLoader>();
			return pluginLoader.GetMetaData(plugin);
		}
	};
}
