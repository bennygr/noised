using System;
using System.Collections.Generic;
using Noised.Core.Media;

namespace Noised.Core.Plugins.Media
{
	/// <summary>
	///		A source to retrieve media items from 
	/// </summary>
	public interface IMediaSource : IPlugin
	{
		/// <summary>
		///		A unique name of the media source
		/// </summary>
		string Identifier{get;}

		/// <summary>
		///		Initializes and/or refreshs the media source
		/// </summary>
		void Refresh();

		/// <summary>
		///		Gets the media item for the given URI
		/// </summary>
		/// <param name="uri">The uri</param>
		/// <returns>The MediaItem for the given Uri, or null if no such MediaItem was found</returns> 
		MediaItem Get(Uri uri);
		
		/// <summary>
		///		Retrieves media items by a search pattern
		/// </summary>
		/// <returns>
		///		An enumeration of media items matching the given pattern
		/// </returns>
		IEnumerable<MediaSourceSearchResult> Search(string pattern);
	};
}
