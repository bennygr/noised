namespace Noised.Core.Media
{
	/// <summary>
	///		Media manager handles playback of media items
	/// </summary>
	public interface IMediaManager
	{
		/// <summary>
		///		Shuffle mode 
		/// </summary>
		bool Shuffle {get;set;}

		/// <summary>
		///		Shuffle mode 
		/// </summary>
		bool Repeat {get;set;}

		/// <summary>
		///		Plays a media item 
		/// </summary>
		void Play(MediaItem item);
	}
}
