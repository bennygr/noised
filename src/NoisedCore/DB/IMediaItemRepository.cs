using System;
using Noised.Core.Media;

namespace Noised.Core.DB
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
        ///		Gets a MediaItem by the item's URL
        /// </summary>
        /// <param name="uri">The URI of the MediaItem to get</param>
        MediaItem GetByUri(Uri uri);
    };
}
