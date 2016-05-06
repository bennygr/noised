namespace Noised.Core.Media
{
    /// <summary>
    ///	Strategy to process the next item from a playlist
    /// </summary>
    interface IProcessPlaylistStrategy
    {
        /// <summary>
        ///     Gets the next item from Playlist
        /// </summary>
        /// <param name="playlist">The playlist to get the next item for</param>
        Listable<MediaItem> GetNextItem(Playlist playlist);
    };
}
