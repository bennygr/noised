namespace Noised.Core.Media
{
    /// <summary>
    /// Interface for a PlaylistManager
    /// </summary>
    public interface IPlaylistManager
    {
        /// <summary>
        /// Gets the loaded Playlist
        /// </summary>
        Playlist LoadedPlaylist { get; }

        /// <summary>
        /// Loads a Playlist
        /// </summary>
        /// <param name="playlist">Playlist to load</param>
        void LoadPlaylist(Playlist playlist);
    }
}
