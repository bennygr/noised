using System.Collections.Generic;
using Noised.Core.Media;
namespace Noised.Core.Media
{
	/// <summary>
	///		A search result for searching a MediaItem
	/// </summary>
	public class MediaSourceSearchResult
	{
		/// <summary>
		///		The name of the media source
		/// </summary>
		public string MediaSourceName{get; private set;}

		/// <summary>
		///		The MediaItem 
		/// </summary>
		public IList<MediaItem> MediaItems{get; private set;}

		/// <summary>
		///		Constructor
		/// </summary>
		/// <param name="mediaSourceName">The name of the media source</param>
		/// <param name="mediaItems">The MediaItems</param>
		public MediaSourceSearchResult (string mediaSourceName, IList<MediaItem> mediaItems)
		{
			MediaSourceName = mediaSourceName;
			MediaItems = mediaItems;
		}
	};
}
