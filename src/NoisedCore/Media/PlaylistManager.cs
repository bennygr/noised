using System.Collections.Generic;
using Noised.Core.DB;

namespace Noised.Core.Media
{
    /// <summary>
    /// Manages all the Playlists
    /// </summary>
    public class PlaylistManager : IPlaylistManager
    {
    public void Reset()
    {
        throw new System.NotImplementedException();
    }
        private readonly object locker = new object();
        private Playlist loadedPlaylist;

        /// <summary>
        /// Gets the currently loaded Playlist
        /// </summary>
        public Playlist LoadedPlaylist
        {
            get
            {
                lock (locker)
                {
                    return loadedPlaylist;
                }
            } 
            private set
            {
                lock (locker)
                {
                    loadedPlaylist = value;
                }
            }
        }
        /// <summary>
        /// Loads a Playlist
        /// </summary>
        /// <param name="playlist">The Playlist to load</param>
        public void LoadPlaylist(Playlist playlist)
        {
            lock (locker)
            {
                playlist.Reset();
                loadedPlaylist = playlist;
            }
        }
    }
}
