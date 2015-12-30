using System;
using System.Collections.Generic;

namespace Noised.Core.Media
{
	/// <summary>
	///		Provides access to all known IMedaSource 
	/// </summary>
	public interface IMediaSourceAccumulator
	{
		/// <summary>
		///		Refreshs all known MediaSources
		/// </summary>
		void Refresh();

		/// <summary>
		///		Retrieves the first MediaItem found by a unique uri
		/// </summary>
		MediaItem GetItem(Uri uri);
	
		/// <summary>
		///		Searches all known IMediaSource's for the given search pattern
		/// </summary>
		/// <returns>
		///		An enumeration of media items matching the given pattern
		/// </returns>
		IEnumerable<MediaItem> Search(string search);
	};
}
