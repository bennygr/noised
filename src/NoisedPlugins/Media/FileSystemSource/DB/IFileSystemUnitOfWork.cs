using System;

namespace Noised.Plugins.Media.FileSystemSource.DB
{
    /// <summary>
    ///		A unit of work for accessing the plugin's databse
    /// </summary>
    interface IFileSystemUnitOfWork : IDisposable
    {
        /// <summary>
        ///		Repository for accessing filesystem media items
        /// </summary>
        IMediaItemRepository MediaItemRepository { get; }

        /// <summary>
        ///		Saves all changes made to the repositories
        /// </summary>
        void SaveChanges();
    };
}
