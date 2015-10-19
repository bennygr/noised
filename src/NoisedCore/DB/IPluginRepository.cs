using System;
using System.Collections.Generic;
using System.IO;
using Noised.Core.Plugins;

namespace Noised.Core.DB
{
	public interface IPluginRepository
	{
		/// <summary>
		///		Registers a new plugin
		/// </summary>
		/// <param name="pluginRegistration">The data of the plugin to register</param>
		/// <param name="files">The files of the plugin to register</param>
		void RegisterPlugin(PluginMetaData pluginRegistration,List<FileInfo> files);	
	
		/// <summary>
		///		Gets the registration data for a plugin
		/// </summary>
		/// <param name="guid">The GUID of the plugin to get the data for</param>
		PluginMetaData GetByGuid(Guid guid);

		/// <summary>
		///		Gets a list of files registered for the plugin
		/// </summary>
		List<FileInfo> GetRegisteredFilesForPlugin(Guid guid);
	};
}
