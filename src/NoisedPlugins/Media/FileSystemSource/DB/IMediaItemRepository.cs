using System;
using System.Collections.Generic;
using Noised.Core.Media;

namespace Noised.Plugins.FileSystemSource.DB
{
    /// <summary>
    ///		Repository for accessing MediaItems
    /// </summary>
    public interface IMediaItemRepository
    {
        /// <summary>
        ///		Creates a new MediaItem
        /// </summary>
        /// <param name="item">The MediaItem to create</param>
        void Create(MediaItem item);

		/// <summary>
		///		Deletes a MediaItem
		/// </summary>
		/// <param name="item">The MediaItem to delete</param>
		void Delete(MediaItem item);

        /// <summary>
        ///		Gets a MediaItem by the item's URL
        /// </summary>
        /// <param name="uri">The URI of the MediaItem to get</param>
        MediaItem GetByUri(Uri uri);

		/// <summary>
		///		Searches for a MediaItem by title
		/// </summary>
		/// <param name="title">The title of the item</param>
		/// <param name="ret">The result list to write found items to</param>
		/// <returns>The count  of found items</returns>
		int FindByTitle(string title,IList<MediaItem> ret);

		/// <summary>
		///		Searches for a MediaItem by artist
		/// </summary>
		/// <param name="artist">The artist of the item</param>
		/// <param name="ret">The result list to write found items to</param>
		/// <returns>The count  of found items</returns>
		int FindByArtist(string artist,IList<MediaItem> ret);

		/// <summary>
		///		Searches for a MediaItem by Album
		/// </summary>
		/// <param name="album">The album of the item</param>
		/// <param name="ret">The result list to write found items to</param>
		/// <returns>The count  of found items</returns>
		int FindByAlbum(string album,IList<MediaItem> ret);
    };
}
