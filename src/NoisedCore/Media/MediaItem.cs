using System;
namespace Noised.Core.Media
{
	/// <summary>
	///		A media item which can be played
	/// </summary>
	public class MediaItem
	{
		/// <summary>
		///		Uri of the media item
		/// </summary>
		public Uri Uri{get; set;}	

		/// <summary>
		///		The meta data of the media item
		/// </summary>
		public MetaData MetaData{get;set;}
	};
}
