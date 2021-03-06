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
		///		Unregisters an existing plugin
		/// </summary>
		/// <param name="pluginRegistration">The data of the plugin to unregister</param>
		void UnregisterPlugin(PluginMetaData pluginRegistration);
	
		/// <summary>
		///		Gets the registration data for a plugin
		/// </summary>
		/// <param name="guid">The GUID of the plugin to get the data for</param>
		PluginMetaData GetByGuid(Guid guid);

		/// <summary>
		///		Gets the registration data for a plugin
		/// </summary>
		/// <param name="file">A file of the plugin to get the data for </param>
		PluginMetaData GetForFile(FileInfo file);

		/// <summary>
		///		Gets all plugin files
		/// </summary>
		/// <param name="pattern">A pattern to restriuct files, or null to get all files</param>
		IList<FileInfo> GetFiles(string pattern=null);
	};
}
