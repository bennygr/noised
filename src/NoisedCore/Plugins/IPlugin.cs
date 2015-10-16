using System;

namespace Noised.Core.Plugins
{
	/// <summary>
	///		A plugin for extending noised
	/// </summary>
	public interface IPlugin : IDisposable
	{
		/// <summary>
		///		A unique and constant identifier of the plugin
		/// </summary>
		/// <remarks>
		///		The returned GUID must always be the same for the plugin
		/// </remarks>
		Guid Guid{get;}

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
