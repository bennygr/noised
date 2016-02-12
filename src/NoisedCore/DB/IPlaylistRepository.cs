using System.Collections.Generic;
using Noised.Core.Media;

namespace Noised.Core.DB
{
    /// <summary>
    /// Interface of the PlaylistRepository
    /// </summary>
    public interface IPlaylistRepository
    {
        /// <summary>
        /// Creates a new Playlist in the current IUnitOfWork implementation
        /// </summary>
        /// <param name="playlist">Playlist to create</param>
        void CreatePlaylist(Playlist playlist);

        /// <summary>
        /// Updates an existing Playlist in the current IUnitOfWork implementation
        /// </summary>
        /// <param name="playlist">Playlist to update</param>
        void UpdatePlaylist(Playlist playlist);

        /// <summary>
        /// Deletes a Playlist in the current IUnitOfWork implementation
        /// </summary>
        /// <param name="playlist">Playlist to delete</param>
        void DeletePlaylist(Playlist playlist);

        /// <summary>
        /// Gets all Playlists
        /// </summary>
        IList<Playlist> AllPlaylists { get; }
    }
}
