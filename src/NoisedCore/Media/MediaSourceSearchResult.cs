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
		public MediaItem MediaItem{get; private set;}
	
		/// <summary>
		///		Constructor
		/// </summary>
		/// <param name="mediaSourceName">The name of the media source</param>
		/// <param name="mediaItem">The MediaItem</param>
		public MediaSourceSearchResult (string mediaSourceName, MediaItem mediaItem)
		{
			MediaSourceName = mediaSourceName;
			MediaItem = mediaItem;
		}
	};
}
