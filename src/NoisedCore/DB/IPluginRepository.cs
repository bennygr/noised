using System;
using Noised.Core.Plugins;

namespace Noised.Core.DB
{
	public interface IPluginRepository
	{
		/// <summary>
		///		Registers a new plugin
		/// </summary>
		/// <param name="pluginRegistration">The data of the plugin to register</param>
		void RegisterPlugin(PluginRegistration pluginRegistration);	
	
		/// <summary>
		///		Gets the registration data for a plugin
		/// </summary>
		/// <param name="guid">The GUID of the plugin to get the data for</param>
		PluginRegistration GetByGuid(Guid guid);
	};
}
