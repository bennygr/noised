using System;

namespace Noised.Core.Plugins
{
	/// <summary>
	///		A plugin for extending noised
	/// </summary>
	public interface IPlugin : IDisposable
	{
		/// <summary>
		///		The name of the plugin
		/// </summary>
		String Name{get;}

		/// <summary>
		///		The description of the plugin
		/// </summary>
		String Description{get;}

		/// <summary>
		///		The name of the author
		/// </summary>
		String AuthorName{get;}

		/// <summary>
		///		The contact of the author
		/// </summary>
		String AuthorContact{get;}

		/// <summary>
		///		The version of the plugin
		/// </summary>
		Version Version{get;}

		/// <summary>
		///		The creation date of the plugin		
		/// </summary>
		DateTime CreationDate{get;}
	};
}
