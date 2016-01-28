using System.Collections.ObjectModel;

namespace Noised.Core.Media
{
    /// <summary>
    /// Interface for a PlaylistManager
    /// </summary>
    public interface IPlaylistManager
    {
        /// <summary>
        /// Gets all known Playlists
        /// </summary>
        ReadOnlyCollection<Playlist> Playlists { get; }

        /// <summary>
        /// Gets the loaded Playlist
        /// </summary>
        Playlist LoadedPlaylist { get; }

        /// <summary>
        /// Creates a Playlist with the given name
        /// </summary>
        /// <param name="name">Name of the Playlist</param>
        /// <returns>The created Playlist</returns>
        Playlist CreatePlaylist(string name);

        /// <summary>
        /// Adds a Playlist to the known Playlists
        /// </summary>
        /// <param name="playlist">Playlist to add</param>
        void AddPlaylist(Playlist playlist);

        /// <summary>
        /// Searches for a Playlist by the name
        /// </summary>
        /// <param name="name">Name of the desired Playlist</param>
        /// <returns>The Playlist with the given name</returns>
        Playlist FindPlaylists(string name);

        /// <summary>
        /// Loads a Playlist
        /// </summary>
        /// <param name="playlist">Playlist to load</param>
        void LoadPlaylist(Playlist playlist);
    }
}