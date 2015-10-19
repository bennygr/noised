using System;

namespace Noised.Core.Plugins
{
	public class PluginMetaData
	{
		/// <summary>
		///		The unique plugin ID
		/// </summary>
		public Guid Guid{get;set;}
	
		/// <summary>
		///		The Plugin Version
		/// </summary>
		public String Version{get;set;}

		/// <summary>
		///		The name of the plugin
		/// </summary>
		public String Name{get;set;}

		/// <summary>
		///		Returns the version of the plugin as parse Version object
		/// </summary>
		public Version GetVersion()
		{
			return System.Version.Parse(Version);
		}
	};
}
