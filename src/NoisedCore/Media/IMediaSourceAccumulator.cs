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
		///		Searches all known IMediaSource's for the given search pattern
		/// </summary>
		/// <param name="pattern">The search pattern</param>
		/// <returns>
		///		An enumeration of search results matching the given pattern
		/// </returns>
		IEnumerable<MediaSourceSearchResult> Search(string pattern);
	};
}
