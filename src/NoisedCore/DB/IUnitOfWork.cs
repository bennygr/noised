using System;

namespace Noised.Core.DB
{
    /// <summary>
    ///		A unit of work for accessing the database
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        ///		Repository for accesing PluginRegistration
        /// </summary>
        IPluginRepository PluginRepository { get; }

        /// <summary>
        /// Repository for Playlists
        /// </summary>
        IPlaylistRepository PlaylistRepository { get; }

        /// <summary>
        ///		Saves all changes made to the repositories 
        /// </summary>
        void SaveChanges();
    };
}
