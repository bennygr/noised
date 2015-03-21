using System.Collections.Generic;
using Noised.Core.Media;

namespace Noised.Core.Plugins.Media
{
	/// <summary>
	///		A source to retrieve media items from 
	/// </summary>
	public interface IMediaSource
	{
		/// <summary>
		///		Retrieves media items by a search pattern
		/// </summary>
		/// <returns>
		///		An enumeration of media items matching the given pattern
		/// </returns>
		IEnumerable<MediaItem> Search(string search);
	};
}
