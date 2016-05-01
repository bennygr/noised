using System;
using System.Collections.Generic;
using Noised.Core.Media;

namespace Noised.Core.DB
{
    /// <summary>
    /// Interface of a PlaylistRepository
    /// </summary>
    public interface IPlaylistRepository
    {
        /// <summary>
        /// Creates a new Playlist in the current IUnitOfWork implementation
        /// </summary>
        /// <param name="playlist">Playlist to create</param>
        void Create(Playlist playlist);

        /// <summary>
        /// Updates an existing Playlist in the current IUnitOfWork implementation
        /// </summary>
        /// <param name="playlist">Playlist to update</param>
        void Update(Playlist playlist);

        /// <summary>
        /// Deletes a Playlist in the current IUnitOfWork implementation
        /// </summary>
        /// <param name="playlist">Playlist to delete</param>
        void Delete(Playlist playlist);

        /// <summary>
        /// Gets all Playlists
        /// </summary>
        /// <returns>A list of all known playlists</returns>
        IList<Playlist> GetAll();

        /// <summary>
        ///     Gets a playlist by its id
        /// </summary>
        Playlist GetById(Int64 id);
    }
}
