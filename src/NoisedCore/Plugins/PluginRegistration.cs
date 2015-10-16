using System;
using System.Collections.Generic;

namespace Noised.Core.Plugins
{
	public class PluginRegistration
	{
		/// <summary>
		///		Tje unique plugin ID
		/// </summary>
		public Guid Guid{get;set;}
	
		/// <summary>
		///		The Plugin Version
		/// </summary>
		public Version Version{get;set;}
	
		/// <summary>
		///		A list of files related to the Plugin
		/// </summary>
		public List<PluginFile> PluginFiles {get;set;}
	};
}
