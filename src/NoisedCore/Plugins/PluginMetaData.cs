using System;

namespace Noised.Core.Plugins
{
	public class PluginMetaData
	{
		/// <summary>
		///		The unique plugin ID
		/// </summary>
		public String Guid{get;set;}
	
		/// <summary>
		///		The Plugin Version
		/// </summary>
		public String Version{get;set;}

		/// <summary>
		///		The name of the plugin
		/// </summary>
		public String Name{get;set;}

		/// <summary>
		///		The description of the plugin
		/// </summary>
		public String Description{get;set;}

		/// <summary>
		///		The name of the plugin author
		/// </summary>
		public String Author{get;set;}

		/// <summary>
		///		The Email contact of the plugin author
		/// </summary>
		public String AuthorEmail{get;set;}

		/// <summary>
		///		The creation date of the plugin
		/// </summary>
		public DateTime? CreationDate{get;set;}

		/// <summary>
		///		Returns the version of the plugin as parse Version object
		/// </summary>
		public Version GetVersion()
		{
			return System.Version.Parse(Version);
		}

		/// <summary>
		///		Returns the Guid of the plugin as Guid object
		/// </summary>
		public Guid GetGuid()
		{
			return System.Guid.Parse(Guid);
		}
	};
}
