using Noised.Core.Media;

namespace Noised.Core.Media
{
    /// <summary>
    ///     Noised's default strategy for processing a playlist
    /// </summary>
    class ProcessPlaylistStrategy : IProcessPlaylistStrategy
    {
        private readonly IMediaManager mediaManager;
    
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="mediaManager">Noised's MediaManager</param>
        public ProcessPlaylistStrategy(IMediaManager mediaManager)
        {
            if (mediaManager == null)
                throw new System.ArgumentNullException("mediaManager");
            this.mediaManager = mediaManager;
        }

        #region IProcessPlaylistStrategy
        
        public Listable<MediaItem> GetNextItem(Playlist playlist)
        {
            if (playlist.HasUnreturnedItems())
            {
                return mediaManager.Shuffle ? playlist.GetNextRandom() : playlist.GetNext();
            }

            //Resetting the playlist if the repeat mode is repeat-playlist
            if (mediaManager.Repeat == RepeatMode.RepeatPlaylist)
            {
                playlist.Reset();
                return mediaManager.Shuffle ? playlist.GetNextRandom() : playlist.GetNext();
            }
            return null;
        }
        
        #endregion
    };
}
